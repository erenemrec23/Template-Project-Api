using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.Menu.Queries.GetList;
using QrAssignment.Application.Features.Menu.Queries.GetUserList;
namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public sealed class ModulesController : ApiControllerBase
    {
         

        [HttpGet("[action]")]
        public async Task<IActionResult> GetSystemModules([FromQuery] GetSystemModulesQuery query, CancellationToken cancellationToken) => HandleResult(await Mediator.Send(query, cancellationToken));

        [HttpGet("GetMenu")]
        public async Task<IActionResult> GetMenu(CancellationToken ct)
    => HandleResult(await Mediator.Send(new GetListMenuQuery(), ct));

        
        [HttpGet("GetUserMenu")]
        public async Task<IActionResult> GetUserMenu(CancellationToken ct)
    => Ok(await Mediator.Send(new GetUserMenuQuery(), ct));
    }
}