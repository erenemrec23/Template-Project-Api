using MediatR;
using QrAssignment.Application.Helpers;
using QrAssignment.Application.Repositories;
using QrAssignment.Application.Services;         // IFileStorageService
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Feedbacks.Commands.Create
{
    public class CreateFeedbackCommandHandler : IRequestHandler<CreateFeedbackCommand, Result<Guid>>
    {
        private readonly IFeedBackRepository _feedbackRepository;
        private readonly IFileStorageService _fileStorage;

        public CreateFeedbackCommandHandler(
            IFeedBackRepository feedbackRepository,
            IFileStorageService fileStorage)
        {
            _feedbackRepository = feedbackRepository;
            _fileStorage = fileStorage;
        }

        public async Task<Result<Guid>> Handle(CreateFeedbackCommand request, CancellationToken ct)
        {
            string? screenshotKey = null;

            if (!string.IsNullOrWhiteSpace(request.ScreenshotBase64))
            {
                var (bytes, contentType, ext) = DataUrlHelper.Parse(request.ScreenshotBase64);
                using var ms = new MemoryStream(bytes);
                var stored = await _fileStorage.SaveAsync(ms, $"screenshot{ext}", "feedbacks", contentType, ct);
                screenshotKey = stored.Key;              // DB'ye base64 değil, bu kısa key gider
            }

            var feedback = new Feedback
            {
                Comment = request.Comment,
                ScreenshotPath = screenshotKey,
                PageUrl = request.PageUrl,
                Status = FeedbackStatus.Pending,
            };

            await _feedbackRepository.AddAsync(feedback, ct);
            return Result.Success(feedback.Id);
        }
    }
}