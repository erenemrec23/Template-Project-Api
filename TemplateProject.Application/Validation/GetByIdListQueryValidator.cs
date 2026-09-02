using FluentValidation;
using Microsoft.Extensions.Localization;
using QrAssignment.Application;
using QrAssignment.Application.Abstractions;

public sealed class GetByIdListQueryValidator<TRequest> : AbstractValidator<TRequest>
    where TRequest : IdListValidationBase
{
    public GetByIdListQueryValidator(IStringLocalizer<SharedResource> localizer)
    {
        // 1. Kural: Listenin kendisi boş (null) veya 0 elemanlı olmamalıdır
        RuleFor(x => x.IdList)
            .NotEmpty()
            .WithMessage(localizer["Error.SelectedItemsCannotBeEmpty"]); // Yerelleştirilmiş hata mesajı anahtarınız

        // 2. Kural: Listenin İÇİNDEKİ her bir ID tek tek senin özel ID kuralından geçmelidir
        RuleForEach(x => x.IdList)
            .MustBeValidId(localizer);
    }
}

 