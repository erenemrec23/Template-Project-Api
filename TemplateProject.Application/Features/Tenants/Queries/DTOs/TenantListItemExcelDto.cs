using QrAssignment.Application.Attributes;
using QrAssignment.Application.Common.Excel;

namespace QrAssignment.Application.Features.Tenants.DTOs
{
    public class TenantListItemExcelDto
    {
        public TenantListItemExcelDto() { }
        public TenantListItemExcelDto(string code, string name)
        {
            Code = code;
            Name = name;
        }
        [ExcelColumn("Excel.Title.Code", Order = 1)]
        public string Code { get; set; }

        [ExcelColumn("Excel.Title.Name", Order = 2)]
        public string Name { get; set; }
    }
}