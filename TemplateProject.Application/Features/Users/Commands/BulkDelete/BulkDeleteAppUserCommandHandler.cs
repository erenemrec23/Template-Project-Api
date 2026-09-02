using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;
using MediatR;

namespace QrAssignment.Application.Features.Users.Commands.BulkDelete
{
    public class BulkDeleteAppUserCommandHandler : IRequestHandler<BulkDeleteAppUserCommand, Result>
    {
        private readonly IAppUserRepository _appUserRepository;

        public BulkDeleteAppUserCommandHandler(IAppUserRepository appUserRepository)
            => _appUserRepository = appUserRepository;

        public async Task<Result> Handle(BulkDeleteAppUserCommand request, CancellationToken cancellationToken)
        {
            await _appUserRepository.BulkDeleteAsync(request.IdList, cancellationToken);
            return Result.Success();
        }
    }
}
