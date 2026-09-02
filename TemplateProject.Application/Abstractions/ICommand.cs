using MediatR;

namespace QrAssignment.Application.Abstractions
{
    public interface ICommand<out TResponse> : IRequest<TResponse>
    {
    }
    public interface ICommand : IRequest<Unit>
    {
    }
}
