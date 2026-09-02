using System.Text.Json.Serialization;

namespace QrAssignment.Application.DTOs // Kendi klasör yapına göre uyarla
{
    public record PermissionDto(
        [property: JsonPropertyName("pageName")] string PageName,
        [property: JsonPropertyName("permissionValue")] int PermissionValue
    );
}