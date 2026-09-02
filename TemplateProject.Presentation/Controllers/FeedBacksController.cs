using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Features.Feedbacks.Commands.Create;
using QrAssignment.Application.Features.Feedbacks.Commands.UpdateStatus; 
using QrAssignment.Application.Features.Feedbacks.Queries.GetList;
using QrAssignment.Application.Features.Tenants.Commands.Update;

namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FeedBacksController : ApiControllerBase
    {

        [HttpPost("create")]
        public async Task<IActionResult> Create(
    [FromForm] string comment,
    [FromForm] string pageUrl,
    IFormFile? screenshot,
    CancellationToken ct)
        {
            string? dataUrl = null;
            if (screenshot is not null)
            {
                using var ms = new MemoryStream();
                await screenshot.CopyToAsync(ms, ct);
                dataUrl = $"data:{screenshot.ContentType};base64,{Convert.ToBase64String(ms.ToArray())}";
            }
            var result = await Mediator.Send(new CreateFeedbackCommand(comment, dataUrl ?? "", pageUrl), ct);
            return HandleResult(result);
        }
         

        [HttpPut("[action]")]
        public async Task<IActionResult> UpdateStatus([FromBody] UpdateFeedbackStatusCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));

        [HttpPost("GetList")]
        public async Task<IActionResult> GetList([FromBody] GetListFeedBackQuery request, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(request, cancellationToken));
    }
}
