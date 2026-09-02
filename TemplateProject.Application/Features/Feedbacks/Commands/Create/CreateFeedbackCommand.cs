// Application/Features/Feedbacks/Commands/Create/CreateFeedbackCommand.cs
using QrAssignment.Application.Abstractions;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Feedbacks.Commands.Create
{
    public record CreateFeedbackCommand(
        string Comment,
        string ScreenshotBase64,
        string PageUrl) : ICommand<Result<Guid>>; 
}