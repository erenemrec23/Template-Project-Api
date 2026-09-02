using QrAssignment.Application.Interfaces;

namespace QrAssignment.Application.Services
{
    internal sealed class PermissionChangeContext : IPermissionChangeContext
    {
        public string? SourcePage { get; set; }
    }
}