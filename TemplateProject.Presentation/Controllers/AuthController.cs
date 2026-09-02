using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.AuthFeatures.Commands.ForgotPassword;
using QrAssignment.Application.Features.AuthFeatures.Commands.Login;
using QrAssignment.Application.Features.AuthFeatures.Commands.LoginTwoFactor;
using QrAssignment.Application.Features.AuthFeatures.Commands.ResetPassword;
using QrAssignment.Application.Features.Users.Commands.Create;

namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [AllowAnonymous]
    public sealed class AuthController : ControllerBase
    {
        private readonly IMediator _mediator;
         
        public AuthController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginCommand command, CancellationToken cancellationToken)
        { 
            var response = await _mediator.Send(command, cancellationToken);
             
            return Ok(response);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] CreateAppUserCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        [HttpPost("forgotpassword")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }

        [HttpPost("resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordCommand command, CancellationToken cancellationToken)
        {
            var result = await _mediator.Send(command, cancellationToken);
            return Ok(result);
        }
        [HttpPost("LoginTwoFactor")] 
        public async Task<IActionResult> LoginTwoFactor([FromBody] LoginTwoFactorCommand command, CancellationToken cancellationToken)
    => Ok(await _mediator.Send(command, cancellationToken));
    }
}
