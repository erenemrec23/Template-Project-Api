using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.PagePermissions.Commands.Update;
using QrAssignment.Application.Features.PagePermissions.Commands.UpdateRolesPermissions;
using QrAssignment.Application.Features.PagePermissions.Commands.UpdateUsersPermissions;
using QrAssignment.Application.Features.PagePermissions.Queries;
using QrAssignment.Application.Features.Permission.Commands.Update;
using QrAssignment.Application.Features.Permission.Queries.GetPagePermissionsForPage;

namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PagePermissionsController : ApiControllerBase
    {
        [HttpGet("[action]")]
        public async Task<IActionResult> GetByPageKey([FromQuery] string pageKey, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetPagePermissionsForPageQuery(pageKey), cancellationToken));

        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdatePagePermissionsForPageCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));

        // Çoklu kullanıcıya toplu yetki (bulk)
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateUsers([FromBody] UpdateUsersPermissionsCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));

        // Çoklu role toplu yetki (bulk)
        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateRoles([FromBody] UpdateRolesPermissionsCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));
    }
}