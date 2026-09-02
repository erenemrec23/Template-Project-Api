using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Features.PagePermissions.DTOs;
using QrAssignment.Application.Security;
using QrAssignment.Domain.Shared;
using System.Data.Entity.Infrastructure;


namespace QrAssignment.Application.Features.PagePermissions.Queries
{
    public sealed record GetPagePermissionsForPageQuery(string PageKey)
    : IPageScopedRequest, IRequest<Result<List<PagePermissionAssignmentDto>>>;

}