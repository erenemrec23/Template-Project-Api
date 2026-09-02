using QrAssignment.Application.Common.Excel;

namespace QrAssignment.Application.Features.Users.Commands.Excel.BulkCreate
{
    public class BulkCreateAppUserInputDto
    {
        [ExcelColumn("Excel.Title.FirstName")]
        [ExcelRequired(ErrorMessageKey = "Excel.Error.FirstNameRequired")]
        [ExcelMaxLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [ExcelColumn("Excel.Title.LastName")]
        [ExcelRequired(ErrorMessageKey = "Excel.Error.LastNameRequired")]
        [ExcelMaxLength(100)]
        public string LastName { get; set; } = string.Empty;

        [ExcelColumn("Excel.Title.UserName")]
        [ExcelRequired(ErrorMessageKey = "Excel.Error.UserNameRequired")]
        [ExcelMaxLength(256)]
        [ExcelUniqueInFile(ErrorMessageKey = "Excel.Error.UserNameDuplicate")]
        public string UserName { get; set; } = string.Empty;

        [ExcelColumn("Excel.Title.Email")]
        [ExcelRequired(ErrorMessageKey = "Excel.Error.EmailRequired")]
        [ExcelMaxLength(256)]
        [ExcelUniqueInFile(ErrorMessageKey = "Excel.Error.EmailDuplicate")]
        public string Email { get; set; } = string.Empty;
    }
}