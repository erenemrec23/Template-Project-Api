using QrAssignment.Application.Common.Excel;

namespace QrAssignment.Application.Features.QrLocations.Commands.Excel.BulkCreate
{
    public class BulkCreateQrLocationInputDto
    {
        [ExcelColumn("Excel.Title.Code", IncludeInSample = false)]
        // [ExcelRequired(ErrorMessageKey = "Excel.Error.CodeRequired")]
        [ExcelUniqueInFile(ErrorMessageKey = "Excel.Error.CodeDuplicate")]
        public long? Code { get; set; }

        [ExcelColumn("Excel.Title.Name")]
        [ExcelRequired(ErrorMessageKey = "Excel.Error.NameRequired")]
        [ExcelMaxLength(200)]
        [ExcelUniqueInFile(ErrorMessageKey = "Excel.Error.QrLocationNameDuplicate")]
        public string Name { get; set; } = string.Empty;

        [ExcelColumn("Excel.Title.StartDate")]
        public DateTimeOffset? StartDate { get; set; }

        [ExcelColumn("Excel.Title.EndDate")]
        public DateTimeOffset? EndDate { get; set; }

        [ExcelColumn("Excel.Title.LocationName")]
        [ExcelMaxLength(200)]
        public string? LocationName { get; set; }
    }
}
