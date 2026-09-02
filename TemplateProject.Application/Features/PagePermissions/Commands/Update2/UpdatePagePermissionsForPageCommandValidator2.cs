// Application/Features/Permission/Commands/UpdatePagePermissionsForPage/UpdatePagePermissionsForPageCommandValidator2.cs
using FluentValidation;
using QrAssignment.Application.Features.PagePermissions.Commands.Update;

namespace QrAssignment.Application.Features.PagePermissions.Commands.Update2
{
    public sealed class UpdatePagePermissionsForPageCommandValidator2
        : AbstractValidator<UpdatePagePermissionsForPageCommand2>
    {
        public UpdatePagePermissionsForPageCommandValidator2()
        {
            RuleFor(x => x.PageKey).NotEmpty();

            RuleForEach(x => x.Permissions).ChildRules(assignment =>
            {
                //assignment.RuleFor(a => a)
                //    .Must(a => (a.UserId.HasValue && !a.RoleId.HasValue)
                //            || (!a.UserId.HasValue && a.RoleId.HasValue))
                //    .WithMessage("Her yetki ataması ya bir kullanıcıya ya da bir role ait olmalıdır, ikisi birden veya hiçbiri olamaz.");
            });
        }
    }
}