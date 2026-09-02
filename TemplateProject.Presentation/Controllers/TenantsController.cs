using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.Tenants.Commands.BulkDelete;
using QrAssignment.Application.Features.Tenants.Commands.BulkSetActive;
using QrAssignment.Application.Features.Tenants.Commands.BulkSetPassive;
using QrAssignment.Application.Features.Tenants.Commands.Create;
using QrAssignment.Application.Features.Tenants.Commands.Delete;
using QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.Tenants.Commands.SetActive;
using QrAssignment.Application.Features.Tenants.Commands.SetPassive;
using QrAssignment.Application.Features.Tenants.Commands.Update;
using QrAssignment.Application.Features.Tenants.Queries.FormBase.GetById;
using QrAssignment.Application.Features.Tenants.Queries.FormBase.GetPassivedById;
using QrAssignment.Application.Features.Tenants.Queries.ListBase.GetList;
using QrAssignment.Application.Features.Tenants.Queries.ListBase.GetListExportExcel;
using QrAssignment.Application.Features.Tenants.Queries.ListBase.GetPassivedList;

namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TenantsController : ApiControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateTenantCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));

        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateTenantCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));


        [HttpPost("export")]
        public async Task<IActionResult> ExportExcel([FromBody] GetListTenantExportExcelQuery query, CancellationToken cancellationToken)
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
            var query = new GetSampleExcelTemplateQuery<BulkCreateTenantInputDto>
            {
                FileName = "tenant-sample-template.xlsx",
                SampleRowCount = 3
            };

            var result = await Mediator.Send(query, cancellationToken);
            if (!result.IsSuccess || result.Value is null)
                return BadRequest(result);

            var file = result.Value;
            return File(file.Data, file.ContentType, file.FileName);
        }


        [HttpPost("validate-excel")]
        public async Task<IActionResult> ValidateExcel(IFormFile file, CancellationToken cancellationToken)
        {
            if (file is null || file.Length == 0)
                return BadRequest("Geçerli bir dosya yüklenmedi.");

            using var memoryStream = new MemoryStream();
            await file.CopyToAsync(memoryStream, cancellationToken);

            var query = new ValidateExcelQuery<BulkCreateTenantInputDto> { FileBytes = memoryStream.ToArray() };
            return HandleResult(await Mediator.Send(query, cancellationToken));
        }

        [HttpPost("bulk-create")]
        public async Task<IActionResult> BulkCreate([FromBody] BulkCreateTenantCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));


        [HttpGet("Passived/{id}")]
        public async Task<IActionResult> GetPassivedById(Guid? id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetPassivedByIdTenantQuery(id), cancellationToken));

        [HttpPost("GetPassivedList")]
        public async Task<IActionResult> GetPassivedList([FromBody] GetPassivedListTenantQuery query, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(query, cancellationToken));

        [HttpPost("GetList")]
        public async Task<IActionResult> GetList([FromBody] GetListTenantQuery request, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(request, cancellationToken));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetByIdTenantQuery(id), cancellationToken));

        [HttpPut("SetActive/{id:guid}")]
        public async Task<IActionResult> SetActive([FromRoute] Guid id, CancellationToken cancellationToken)
    => HandleResult(await Mediator.Send(new SetActiveTenantCommand { Id = id }, cancellationToken));

        [HttpPut("SetPassive/{id:guid}")]
        public async Task<IActionResult> SetPassive([FromRoute] Guid id, CancellationToken cancellationToken)
    => HandleResult(await Mediator.Send(new SetPassiveTenantCommand { Id = id }, cancellationToken));


        [HttpPatch("Bulk-Delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteTenantCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new DeleteTenantCommand { Id = id }, cancellationToken));

        [HttpPatch("Bulk-SetActive")]
        public async Task<IActionResult> BulkSetActive([FromBody] BulkSetActiveTenantCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));


        [HttpPatch("Bulk-SetPassive")]
        public async Task<IActionResult> BulkSetPassive([FromBody] BulkSetPassiveTenantCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));

    }
}
