using FluentValidation;
using Microsoft.Extensions.Localization;
using QrAssignment.Application;
using QrAssignment.Application.Abstractions;

public sealed class GetByIdQueryValidator<TRequest> : AbstractValidator<TRequest>
    where TRequest : IdValidationBase
{
    public GetByIdQueryValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.Id).MustBeValidId(localizer);
    }
}

