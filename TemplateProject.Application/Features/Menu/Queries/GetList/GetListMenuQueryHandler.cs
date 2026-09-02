// Application/Features/Modules/Queries/GetSystemModules/PageCatalogItemDto.cs
using MediatR;
using QrAssignment.Application.Repositories;
using QrAssignment.Domain.Shared;
using QrAssignment.Application.Features.Menu.Queries.DTOs;

namespace QrAssignment.Application.Features.Menu.Queries.GetList
{
    public sealed class GetListMenuQueryHandler
    : IRequestHandler<GetListMenuQuery, Result<List<MenuGroupDto>>>
    {
        private readonly IPageRepository _pageRepository;
        public GetListMenuQueryHandler(IPageRepository pageRepository)
            => _pageRepository = pageRepository;

        public async Task<Result<List<MenuGroupDto>>> Handle(
            GetListMenuQuery request, CancellationToken ct)
            => Result.Success(await _pageRepository.GetMenuAsync(ct));
    }
}