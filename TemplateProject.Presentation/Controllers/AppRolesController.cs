using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.Roles.Commands.BulkDelete;
using QrAssignment.Application.Features.Roles.Commands.BulkSetActive;
using QrAssignment.Application.Features.Roles.Commands.BulkSetPassive;
using QrAssignment.Application.Features.Roles.Commands.Create;
using QrAssignment.Application.Features.Roles.Commands.Delete;
using QrAssignment.Application.Features.Roles.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.Roles.Commands.SetActive;
using QrAssignment.Application.Features.Roles.Commands.SetPassive;
using QrAssignment.Application.Features.Roles.Commands.Update;
using QrAssignment.Application.Features.Roles.Queries.FormBase.GetById;
using QrAssignment.Application.Features.Roles.Queries.FormBase.GetPassivedById;
using QrAssignment.Application.Features.Roles.Queries.GetAssignedPermissionList;
using QrAssignment.Application.Features.Roles.Queries.ListBase.GetList;
using QrAssignment.Application.Features.Roles.Queries.ListBase.GetListExportExcel;
using QrAssignment.Application.Features.Roles.Queries.ListBase.GetPassivedList;
using QrAssignment.Application.Features.Roles.Queries.LookUp.GetAssignedUserList;
using QrAssignment.Application.Features.Roles.Queries.LookUp.GetRoleLookUp;
using QrAssignment.Application.Features.Roles.Queries.LookUp.GetRoleLookUpWithPermission;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Presentation.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AppRolesController : ApiControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> Create(CreateAppRoleCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));

        [HttpPut("[action]")]
        public async Task<IActionResult> Update([FromBody] UpdateAppRoleCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));

        

        [HttpPost("export")]
        public async Task<IActionResult> ExportExcel([FromBody] GetListAppRoleExportExcelQuery query, CancellationToken cancellationToken)
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
            var query = new GetSampleExcelTemplateQuery<BulkCreateAppRoleInputDto>
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

            var query = new ValidateExcelQuery<BulkCreateAppRoleInputDto> { FileBytes = memoryStream.ToArray() };
            return HandleResult(await Mediator.Send(query, cancellationToken));
        }

        [HttpPost("bulk-create")]
        public async Task<IActionResult> BulkCreate([FromBody] BulkCreateAppRoleCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));
         

        [HttpGet("Passived/{roleId}")]
        public async Task<IActionResult> GetPassivedById(Guid? id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetPassivedByIdAppRoleQuery(id), cancellationToken));

        [HttpPost("GetPassivedList")]
        public async Task<IActionResult> GetPassivedList([FromBody] GetPassivedListAppRoleQuery query, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(query, cancellationToken));

        [HttpPost("GetList")]
        public async Task<IActionResult> GetList([FromBody] GetListAppRoleQuery request, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(request, cancellationToken));

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(new GetByIdRoleQuery(id), cancellationToken));

          
        [HttpGet("[action]")]
        public async Task<IActionResult> GetAssignedUserList(
    [FromQuery] Guid? roleId, CancellationToken cancellationToken)
    => HandleResult(await Mediator.Send(new GetRoleAssignedUserListQuery(roleId), cancellationToken));

        [HttpGet("[action]")]
        public async Task<IActionResult> GetAssignedPermissionList(
    [FromQuery] Guid? roleId, CancellationToken cancellationToken)
    => HandleResult(await Mediator.Send(new GetRoleAssignedPermissionListQuery(roleId), cancellationToken));


        [HttpPut("SetPassive/{id:guid}")]
        public async Task<IActionResult> SetPassive([FromRoute] Guid id, CancellationToken cancellationToken)
    => HandleResult(await Mediator.Send(new SetPassiveAppRoleCommand(id), cancellationToken));


        [HttpPut("SetActive/{id:guid}")]
        public async Task<IActionResult> SetActive(Guid id, CancellationToken cancellationToken)
    => HandleResult(await Mediator.Send(new SetActiveAppRoleCommand(id), cancellationToken));


        [HttpPatch("Bulk-Delete")]
        public async Task<IActionResult> BulkDelete([FromBody] BulkDeleteAppRoleCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));


        [HttpPatch("Bulk-SetPassive")]
        public async Task<IActionResult> BulkSetPassive([FromBody] BulkSetPassiveAppRoleCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));


        [HttpPatch("Bulk-SetActive")]
        public async Task<IActionResult> BulkSetActive([FromBody] BulkSetActiveAppRoleCommand command, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(command, cancellationToken));


        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete([FromRoute] Guid? id, CancellationToken cancellationToken)
              => HandleResult(await Mediator.Send(new DeleteAppRoleCommand(id), cancellationToken));


        [HttpPost("GetRoleLookUpWithPermission")]
        public async Task<IActionResult> GetRoleLookUpWithPermission([FromBody] GetRoleLookUpWithPermissionQuery query, CancellationToken cancellationToken)
            => HandleResult(await Mediator.Send(query, cancellationToken));



        [HttpGet("[action]")]
        public async Task<IActionResult> GetRoleLookUpList([FromQuery] GetRoleLookUpQuery query, CancellationToken cancellationToken)
    => HandleResult(await Mediator.Send(query, cancellationToken));



    }
}