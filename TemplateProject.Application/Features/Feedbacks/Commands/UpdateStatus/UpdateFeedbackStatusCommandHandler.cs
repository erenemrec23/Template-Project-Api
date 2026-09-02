// Application/Features/Feedbacks/Commands/UpdateStatus/UpdateFeedbackStatusCommandHandler.cs
using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Services; // Email Service
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Feedbacks.Commands.UpdateStatus
{
    public class UpdateFeedbackStatusCommandHandler : IRequestHandler<UpdateFeedbackStatusCommand, Result>
    {
        private readonly IGenericRepository<Feedback> _repository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IEmailService _emailService;

        public UpdateFeedbackStatusCommandHandler(
            IGenericRepository<Feedback> repository,
            IUnitOfWork unitOfWork,
            IEmailService emailService)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
            _emailService = emailService;
        }

        public async Task<Result> Handle(UpdateFeedbackStatusCommand request, CancellationToken cancellationToken)
        {
            var feedback = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (feedback is null)
                return Result.Failure(new Error("Feedback.NotFound", "Geri bildirim bulunamadı."));

            var previousStatus = feedback.Status;
            feedback.Status = request.Status;

            _repository.Update(feedback);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            // DURUM 'COMPLETED' OLDUĞUNDA MAİL AT
            if (previousStatus != FeedbackStatus.Completed && request.Status == FeedbackStatus.Completed)
            {
                if (!string.IsNullOrEmpty(feedback.CreatedByUser.UserName))
                {
                    string subject = "Geri Bildiriminiz Tamamlandı";
                    string body = $@"
                        <h3>Merhaba,</h3>
                        <p>Daha önce iletmiş olduğunuz geri bildirim incelenmiş ve <b>Tamamlandı</b> olarak işaretlenmiştir.</p>
                        <p><b>Geri Bildiriminiz:</b> {feedback.Comment}</p>
                        <p>Teşekkür ederiz.</p>";

                    await _emailService.SendEmailAsync(feedback.CreatedByUser.UserName, subject, body);
                }
            }

            return Result.Success();
        }
    }
}