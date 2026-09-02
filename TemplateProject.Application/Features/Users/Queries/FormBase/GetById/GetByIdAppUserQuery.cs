using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.Users.Queries.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.FormBase.GetById
{
    public sealed record GetByIdAppUserQuery(Guid? Id) : ICommand<Result<AppUserItemDto>>;
}
