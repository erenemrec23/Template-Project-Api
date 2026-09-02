using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.Users.Commands.Excel.BulkCreate;
using QrAssignment.Application.Interfaces;

namespace QrAssignment.Application.Features.Users.Commands.Excel.Validate
{
    public sealed class ValidateAppUserExcelQueryHandler
        : ValidateExcelQueryHandlerBase<BulkCreateAppUserInputDto>
    {
        public ValidateAppUserExcelQueryHandler(
            IEnumerable<IExcelRowBusinessValidator<BulkCreateAppUserInputDto>> validators,
            ILocalizationService localizationService)
            : base(validators, localizationService) { }
    }
}