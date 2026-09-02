using QrAssignment.Application.Attributes;
using QrAssignment.Application.Common.Excel;

namespace QrAssignment.Application.Features.Roles.DTOs
{
    public class RoleListItemExcelDto
    { 
        public RoleListItemExcelDto(string name) => Name = name;

        [ExcelColumn("Excel.Title.RoleName", Order = 1)]
        public string Name { get; set; }
    }
}