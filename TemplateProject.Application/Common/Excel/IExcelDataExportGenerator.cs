namespace QrAssignment.Application.Common.Excel;

public interface IExcelDataExportGenerator
{
    byte[] Generate<TDto>(IEnumerable<TDto> data) where TDto : class;
}