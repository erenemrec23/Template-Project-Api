using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.QrLocations.Commands.Delete
{
    public class DeleteQrLocationCommand : ICommand<Result>, IdValidationBase
    {
        public Guid? Id { get; set; }
    }
}
