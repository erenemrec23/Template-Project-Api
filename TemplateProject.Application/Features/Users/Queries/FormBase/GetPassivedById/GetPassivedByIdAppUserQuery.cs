using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.FormBase.GetPassivedById
{
    public sealed record GetPassivedByIdAppUserQuery(Guid? Id) : ICommand<Result<AppUserItemDto>>;
}
