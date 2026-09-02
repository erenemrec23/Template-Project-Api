using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Common.Excel
{
    public class ExcelRowResultDto<TDto>
    {
        public int RowNumber { get; set; }
        public TDto? Data { get; set; }
        public bool IsValid { get; set; } = true;
        public List<string> Errors { get; set; } = new();
    }

    public class ExcelValidationResponseDto<TDto> where TDto : class
    {
        public List<ExcelRowResultDto<TDto>> Rows { get; set; } = new();
        public int TotalRowCount { get; set; }

        public IEnumerable<ExcelRowResultDto<TDto>> ValidRows => Rows.Where(r => r.IsValid);
        public IEnumerable<ExcelRowResultDto<TDto>> InvalidRows => Rows.Where(r => !r.IsValid);
        public bool HasError => Rows.Any(r => !r.IsValid);
    }
}
