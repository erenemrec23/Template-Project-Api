// Application/Features/Feedbacks/Commands/UpdateStatus/UpdateFeedbackStatusCommand.cs
using MediatR;
using QrAssignment.Domain.Entity.App;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Feedbacks.Commands.UpdateStatus
{
    public record UpdateFeedbackStatusCommand(Guid Id, FeedbackStatus Status) : IRequest<Result>;
}