using QrAssignment.Application.Common.Excel;
using System;
using System.Collections.Generic;
using System.Text;

namespace QrAssignment.Application.Interfaces
{
    public interface IExcelRowBusinessValidator<TDto> where TDto : class
    {
        Task ValidateAsync(IReadOnlyList<ExcelRowResultDto<TDto>> rows, CancellationToken cancellationToken);
    }
}
