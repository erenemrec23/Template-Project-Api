using QrAssignment.Application.Common.Excel;

namespace QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate
{
    public class BulkCreateTenantInputDto
    {
        [ExcelColumn("Excel.Title.Code", IncludeInSample = false)]
        // [ExcelRequired(ErrorMessageKey = "Excel.Error.CodeRequired")]
        [ExcelUniqueInFile(ErrorMessageKey = "Excel.Error.CodeDuplicate")] 
        public long? Code { get; set; }

        [ExcelColumn("Excel.Title.Name")]
        [ExcelRequired(ErrorMessageKey = "Excel.Error.NameRequired")]
        [ExcelMaxLength(200)]
        [ExcelUniqueInFile(ErrorMessageKey = "Excel.Error.TenantNameDuplicate")]
        public string Name { get; set; } = string.Empty;
    }
}