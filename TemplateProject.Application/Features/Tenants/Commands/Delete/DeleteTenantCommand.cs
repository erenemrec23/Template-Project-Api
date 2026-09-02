using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Vml.Spreadsheet;
using MediatR;
using QrAssignment.Application.Abstractions;
using QrAssignment.Application.Interfaces;
using QrAssignment.Domain.Shared;

namespace QrAssignment.Application.Features.Tenants.Commands.Delete
{
    public class DeleteTenantCommand : ICommand<Result>, IdValidationBase
    {

        public Guid? Id { get; set; }
    }
}

