namespace QrAssignment.Application.Interfaces
{
    public interface IExcelTemplateGenerator
    {
        byte[] Generate<TDto>(string languageCode) where TDto : class, new();
    }
}
