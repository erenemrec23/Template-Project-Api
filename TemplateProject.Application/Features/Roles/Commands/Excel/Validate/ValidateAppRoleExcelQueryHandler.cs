using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.Roles.Commands.Excel.BulkCreate;
using QrAssignment.Application.Interfaces;

namespace QrAssignment.Application.Features.Roles.Commands.Excel.Validate
{
    public sealed class ValidateAppRoleExcelQueryHandler
        : ValidateExcelQueryHandlerBase<BulkCreateAppRoleInputDto>
    {
        public ValidateAppRoleExcelQueryHandler(
            IEnumerable<IExcelRowBusinessValidator<BulkCreateAppRoleInputDto>> validators,
            ILocalizationService localizationService)
            : base(validators, localizationService) { }
    }
}