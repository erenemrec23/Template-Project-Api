using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.Profile.Commands;
using QrAssignment.Application.Features.Profile.Commands.TwoFactor;
using QrAssignment.Application.Features.Profile.Commands.Update;
using QrAssignment.Application.Features.Profile.Queries;

namespace QrAssignment.WebApi.Controllers // <-- kendi API namespace'iniz
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] // sadece giris yapmis kullanici; PagePermissions gerektirmez
    public sealed class ProfileController : ControllerBase
    {
        private readonly IMediator _mediator;
        public ProfileController(IMediator mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> Get() => Ok(await _mediator.Send(new GetProfileQuery()));

        [HttpPut]
        public async Task<IActionResult> Update([FromBody] UpdateProfileCommand command) => Ok(await _mediator.Send(command));

        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] ChangePasswordCommand command) => Ok(await _mediator.Send(command));

        [HttpGet("TwoFactor")]
        public async Task<IActionResult> GetTwoFactor()
        {
            var p = await _mediator.Send(new GetProfileQuery());
            return Ok(p); // TwoFactorEnabled zaten ProfileDto icinde
        }

        [HttpPost("TwoFactor/Setup")]
        public async Task<IActionResult> SetupTwoFactor() => Ok(await _mediator.Send(new BeginTwoFactorSetupCommand()));

        [HttpPost("TwoFactor/Enable")]
        public async Task<IActionResult> EnableTwoFactor([FromBody] EnableTwoFactorCommand command) => Ok(await _mediator.Send(command));

        [HttpPost("TwoFactor/Disable")]
        public async Task<IActionResult> DisableTwoFactor() => Ok(await _mediator.Send(new DisableTwoFactorCommand()));
    }
}