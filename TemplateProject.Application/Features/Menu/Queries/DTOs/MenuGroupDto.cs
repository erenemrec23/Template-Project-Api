

namespace QrAssignment.Application.Features.Menu.Queries.DTOs
{
    public sealed record MenuGroupDto(string Key, string Icon, List<MenuPageDto> Children);
}