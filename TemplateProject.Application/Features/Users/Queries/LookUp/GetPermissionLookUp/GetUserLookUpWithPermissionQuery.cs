using MediatR;
using QrAssignment.Application.DTOs.List;
using QrAssignment.Application.Features.Users.Queries.LookUp.DTOs;
using QrAssignment.Application.Security;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Users.Queries.LookUp.GetPermissionLookUp
{  

    public enum PermissionFilter { All = 0, WithPermission = 1, WithoutPermission = 2 }

    public sealed class GetUserLookUpWithPermissionQuery
         : PageRequestBaseDto,
           IRequest<Result<Paginate<PermissionLookUpListItemDto>>>,
           IPageScopedRequest
    { 
        public string PageKey { get; set; } = default!;
         
        public PermissionFilter Filter { get; set; } = PermissionFilter.All;
        public string? Name { get; set; }
        public string? SortBy { get; set; } = "HasPermission";

        public string? SortDirection { get; set; } = "desc";
    }



}