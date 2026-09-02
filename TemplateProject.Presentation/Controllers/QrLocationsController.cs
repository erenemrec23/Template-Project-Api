using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.QrLocations.Commands.BulkDelete;
using QrAssignment.Application.Features.QrLocations.Commands.BulkSetActive;
using QrAssignment.Application.Features.QrLocations.Commands.BulkSetPassive;
using QrAssignment.Application.Features.QrLocations.Commands.Create;
using QrAssignment.Application.Features.QrLocations.Commands.Delete;
using QrAssignment.Application.Features.QrLocations.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.QrLocations.Commands.Excel.Validate;
using QrAssignment.Application.Features.QrLocations.Commands.SetActive;
using QrAssignment.Application.Features.QrLocations.Commands.SetPassive;
using QrAssignment.Application.Features.QrLocations.Commands.Update;
using QrAssignment.Application.Features.QrLocations.Queries.FormBase.GetById;
using QrAssignment.Application.Features.QrLocations.Queries.FormBase.GetPassivedById;
using QrAssignment.Application.Features.QrLocations.Queries.ListBase.GetList;
using QrAssignment.Application.Features.QrLocations.Queries.ListBase.GetListExportExcel;
using QrAssignment.Application.Features.QrLocations.Queries.ListBase.GetPassivedList;

namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QrLocationsController : ApiControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateQrLocationCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));

        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateQrLocationCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));


        [HttpPost("export")]
        public async Task<IActionResult> ExportExcel([FromBody] GetListQrLocationExportExcelQuery query, CancellationToken cancellationToken)
        {
            var result = await Mediator.Send(query, cancellationToken);
            if (!result.IsSuccess || result.Value is null)
                return BadRequest(result);

            var file = result.Value;
            return File(file.Data, file.ContentType, file.FileName);
        }

        [HttpGet("sample-export")]
        public async Task<IActionResult> ExportSampleExcel(CancellationToken cancellationToken)
        {
            var query = new GetSampleExcelTemplateQuery<BulkCreateQrLocationInputDto>
            {
                FileName = "qrlocation-sample-template.xlsx",
                SampleRowCount = 3
            };

            var result = await Mediator.Send(query, cancellationToken);
            if (!result.IsSuccess || result.Value is null)
                return BadRequest(result);

            var file = result.Value;
            return File(file.Data, file.ContentType, file.FileName);
        }


        // NOT: Tenant controller'ında bu endpoint generic ValidateExcelQuery<T> gönderiyordu.
        // Burada entity'ye özel ValidateQrLocationExcelQuery kullanıldı; böylece
        // IExcelRowBusinessValidator<BulkCreateQrLocationInputDto> iş-kuralı doğrulayıcıları
        // (ör. isim tekilliği) DI üzerinden pipeline'a dahil olur.
        [HttpPost("validate-excel")]
        public async Task<IActionResult> ValidateExcel(IFormFile file, CancellationToken cancellationToken)
        {
            if (file is null || file.Length == 0)
                return BadRequest("Geçerli bir dosya yüklenmedi.");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream, cancellationToken);

            var query = new ValidateQrLocationExcelQuery { FileBytes = memoryStream.ToArray() };
            return HandleResult(await Mediator.Send(query, cancellationToken));
        }

        [HttpPost("bulk-create")]
        public async Task<IActionResult> BulkCreate([FromBody] BulkCreateQrLocationCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));


        [HttpGet("Passived/{id}")]
        public async Task<IActionResult> GetPassivedById(Guid? id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetPassivedByIdQrLocationQuery(id), cancellationToken));

        [HttpPost("GetPassivedList")]
        public async Task<IActionResult> GetPassivedList([FromBody] GetPassivedListQrLocationQuery query, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(query, cancellationToken));

        [HttpPost("GetList")]
        public async Task<IActionResult> GetList([FromBody] GetListQrLocationQuery request, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(request, cancellationToken));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetByIdQrLocationQuery(id), cancellationToken));

        [HttpPut("SetActive/{id:guid}")]
        public async Task<IActionResult> SetActive([FromRoute] Guid id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new SetActiveQrLocationCommand { Id = id }, cancellationToken));

        [HttpPut("SetPassive/{id:guid}")]
        public async Task<IActionResult> SetPassive([FromRoute] Guid id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new SetPassiveQrLocationCommand { Id = id }, cancellationToken));


        [HttpPatch("Bulk-Delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteQrLocationCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new DeleteQrLocationCommand { Id = id }, cancellationToken));

        [HttpPatch("Bulk-SetActive")]
        public async Task<IActionResult> BulkSetActive([FromBody] BulkSetActiveQrLocationCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));


        [HttpPatch("Bulk-SetPassive")]
        public async Task<IActionResult> BulkSetPassive([FromBody] BulkSetPassiveQrLocationCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));
    }
}
