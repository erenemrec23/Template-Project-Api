using QrAssignment.Application.Attributes;
using QrAssignment.Application.Common.Excel;

namespace QrAssignment.Application.Features.Users.DTOs
{
    public class AppUserListItemExcelDto
    {
        public AppUserListItemExcelDto(string fullName, string email)
        {
            FullName = fullName;
            Email = email;
        }

        [ExcelColumn("Excel.Title.FullName", Order = 1)]
        public string FullName { get; set; }

        [ExcelColumn("Excel.Title.Email", Order = 2)]
        public string Email { get; set; }
    }
}
