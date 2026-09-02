using MediatR; 
using QrAssignment.Domain.Shared;              // Result<T>

namespace QrAssignment.Application.Common.Excel;

public sealed class GetSampleExcelTemplateQuery<TDto> : IRequest<Result<ExcelFileDto>>
    where TDto : class
{
    public int SampleRowCount { get; init; } = 3;
    public string FileName { get; init; } = "sample-template.xlsx";
}

internal sealed class GetSampleExcelTemplateQueryHandler<TDto>
    : IRequestHandler<GetSampleExcelTemplateQuery<TDto>, Result<ExcelFileDto>>
    where TDto : class
{
    private const string XlsxContentType =
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

    private readonly IExcelSampleTemplateGenerator _generator;
    public GetSampleExcelTemplateQueryHandler(IExcelSampleTemplateGenerator generator)
        => _generator = generator;

    public Task<Result<ExcelFileDto>> Handle(
        GetSampleExcelTemplateQuery<TDto> request, CancellationToken cancellationToken)
    {
        var bytes = _generator.Generate<TDto>(request.SampleRowCount);

        var file = new ExcelFileDto
        {
            Data = bytes,
            FileName = request.FileName,
            ContentType = XlsxContentType
        };

        return Task.FromResult(Result<ExcelFileDto>.Success(file));
    }
}