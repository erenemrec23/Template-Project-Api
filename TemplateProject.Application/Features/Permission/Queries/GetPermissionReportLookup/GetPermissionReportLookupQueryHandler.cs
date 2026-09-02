using MediatR;
using QrAssignment.Application.Features.Permission.Queries.DTOs;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Permission.Queries.GetPermissionReportLookup;

public sealed class GetPermissionReportLookupQueryHandler
    : IRequestHandler<GetPermissionReportLookupQuery, Result<PermissionReportLookupDto>>
{
    private readonly IPagePermissionReportRepository _repo;
    public GetPermissionReportLookupQueryHandler(IPagePermissionReportRepository repo) => _repo = repo;

    public async Task<Result<PermissionReportLookupDto>> Handle(GetPermissionReportLookupQuery _, CancellationToken ct)
        => Result.Success(await _repo.GetLookupsAsync(ct));
}