using QrAssignment.Application.Attributes;
using QrAssignment.Application.Common.Excel;

namespace QrAssignment.Application.Features.QrLocations.DTOs
{
    public class QrLocationListItemExcelDto
    {
        public QrLocationListItemExcelDto() { }

        public QrLocationListItemExcelDto(string code, string name, DateTimeOffset? startDate,
            DateTimeOffset? endDate, string? locationName)
        {
            Code = code;
            Name = name;
            StartDate = startDate;
            EndDate = endDate;
            LocationName = locationName;
        }

        [ExcelColumn("Excel.Title.Code", Order = 1)]
        public string Code { get; set; }

        [ExcelColumn("Excel.Title.Name", Order = 2)]
        public string Name { get; set; }

        [ExcelColumn("Excel.Title.StartDate", Order = 3)]
        public DateTimeOffset? StartDate { get; set; }

        [ExcelColumn("Excel.Title.EndDate", Order = 4)]
        public DateTimeOffset? EndDate { get; set; }

        [ExcelColumn("Excel.Title.LocationName", Order = 5)]
        public string? LocationName { get; set; }
    }
}
