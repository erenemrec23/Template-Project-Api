using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.SetActive
{
    public class SetActiveTenantCommand : ICommand<Result>, IdValidationBase
    {
        public Guid? Id { get; set; }
    }
}