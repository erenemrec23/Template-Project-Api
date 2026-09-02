using MediatR;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.Permission.Commands.Update;
using QrAssignment.Application.Features.Permission.Queries.GetByUserId;


namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppUserPermissionsController : ControllerBase
    {
        private readonly IMediator _mediator;

        // Sadece IMediator'ı enjekte ediyoruz, AuthService veya JwtProvider ile işimiz yok!
        public AppUserPermissionsController(IMediator mediator)
        {
            _mediator = mediator;
        }
         
        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateUserPermissionCommand command)
        {
            var result = await _mediator.Send(command);
            return Ok(result);
        }


        [HttpGet("[action]")]
        public async Task<IActionResult> GetListByUserId(Guid? userId, CancellationToken cancellationToken)
        {
            var query = new GetByIdPermissionUserQuery(userId);
            var result = await _mediator.Send(query, cancellationToken);

            if (!result.IsSuccess)
                return BadRequest(result);

            return Ok(result);
        }
        
    }
}
