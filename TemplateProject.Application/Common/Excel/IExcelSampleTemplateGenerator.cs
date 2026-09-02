namespace QrAssignment.Application.Common.Excel;

public interface IExcelSampleTemplateGenerator
{
    byte[] Generate<TDto>(int sampleRowCount = 3) where TDto : class;
}