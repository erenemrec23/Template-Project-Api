using QrAssignment.Application.Common.Excel;

namespace QrAssignment.Application.Features.Roles.Commands.Excel.BulkCreate
{
    public class BulkCreateAppRoleInputDto
    {
        [ExcelColumn("Excel.Title.RoleName")]
        [ExcelRequired(ErrorMessageKey = "Excel.Error.RoleNameRequired")]
        [ExcelMaxLength(256)]                                  
        [ExcelUniqueInFile(ErrorMessageKey = "Excel.Error.RoleNameDuplicate")]
        public string Name { get; set; } = string.Empty;
    }
}