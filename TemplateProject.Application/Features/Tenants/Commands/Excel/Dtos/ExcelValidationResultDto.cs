using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Features.Tenants.Commands.Excel.Dtos 
{
    public class ExcelTenantRowResultDto
    {
        public int RowNumber { get; set; }
        public long? Code { get; set; } 
        public string? Name { get; set; } 
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
     

    public class ExcelValidationResponseDto<TDto> where TDto : class
    {
        public List<TDto> ValidRows { get; set; } = new();
        public List<ExcelRowErrorDto> InvalidRows { get; set; } = new();
        public int TotalRowCount { get; set; }
        public bool HasError => InvalidRows.Count > 0;
    }
    public class ExcelRowErrorDto
    {
        public int RowNumber { get; set; }
        public string? ColumnName { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;
    }
}
