using MediatR;
using QrAssignment.Application.Features.Permission.Queries.DTOs;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Permission.Queries.GetPermissionReportLookup;

public sealed record GetPermissionReportLookupQuery : IRequest<Result<PermissionReportLookupDto>>;
