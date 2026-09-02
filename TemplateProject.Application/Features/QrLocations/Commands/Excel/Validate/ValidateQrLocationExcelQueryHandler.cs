using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.QrLocations.Commands.Excel.BulkCreate;
using QrAssignment.Application.Interfaces;

namespace QrAssignment.Application.Features.QrLocations.Commands.Excel.Validate
{
    public sealed class ValidateQrLocationExcelQueryHandler
        : ValidateExcelQueryHandlerBase<BulkCreateQrLocationInputDto>
    {
        public ValidateQrLocationExcelQueryHandler(
            IEnumerable<IExcelRowBusinessValidator<BulkCreateQrLocationInputDto>> validators,
            ILocalizationService localizationService)
            : base(validators, localizationService) { }
    }
}
