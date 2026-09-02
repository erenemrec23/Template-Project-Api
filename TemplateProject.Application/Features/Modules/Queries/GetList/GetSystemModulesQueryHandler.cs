// Application/Features/Modules/Queries/GetSystemModules/PageCatalogItemDto.cs
using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;

// GetSystemModulesQueryHandler.cs
public sealed class GetSystemModulesQueryHandler
    : IRequestHandler<GetSystemModulesQuery, Result<List<PageCatalogItemDto>>>
{
    private readonly IPageRepository _pageRepository;
    public GetSystemModulesQueryHandler(IPageRepository pageRepository)
        => _pageRepository = pageRepository;

    public async Task<Result<List<PageCatalogItemDto>>> Handle(
        GetSystemModulesQuery request, CancellationToken ct)
        => Result.Success(await _pageRepository.GetCatalogAsync(request.PageKey, ct));
}
