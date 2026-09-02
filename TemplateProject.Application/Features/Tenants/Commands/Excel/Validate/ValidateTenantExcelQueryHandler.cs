using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate;
using QrAssignment.Application.Interfaces;

namespace QrAssignment.Application.Features.Tenants.Commands.Excel.Validate
{
    public sealed class ValidateTenantExcelQueryHandler
    : ValidateExcelQueryHandlerBase<BulkCreateTenantInputDto>
    {
        public ValidateTenantExcelQueryHandler(
            IEnumerable<IExcelRowBusinessValidator<BulkCreateTenantInputDto>> validators,
            ILocalizationService localizationService)
            : base(validators, localizationService) { }
    }
}
