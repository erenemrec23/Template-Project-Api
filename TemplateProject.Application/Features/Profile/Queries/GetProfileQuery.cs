using MediatR;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Profile.Queries
{
    // Sizde IQuery<T> marker'i varsa IRequest yerine onu kullanin.
    public sealed record ProfileDto(string FirstName, string LastName, string Email, bool TwoFactorEnabled);

    public sealed record GetProfileQuery() : IRequest<Result<ProfileDto>>;
}