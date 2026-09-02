using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using QrAssignment.Application.Behaviors;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Features.QrLocations.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.Roles.Commands.Excel.BulkCreate;
using QrAssignment.Application.Features.Tenants.Commands.Excel.BulkCreate;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Services;
using QrAssignment.Domain.Shared;
using System.Reflection; 

namespace QrAssignment.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services)
        {
            services.AddAutoMapper(cfg => { }, typeof(DependencyInjection));

            services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly());

                cfg.AddOpenBehavior(typeof(AuthorizationBehavior<,>));
                cfg.AddOpenBehavior(typeof(ValidationBehavior<,>));
                cfg.AddOpenBehavior(typeof(UnitOfWorkBehavior<,>));
            });
            services.AddSampleTemplateHandler<BulkCreateAppRoleInputDto>();
            // Açık generic handler'lar MediatR taramasıyla bulunamaz → kapalı tiple elle kaydet.
            services.AddSampleTemplateHandler<BulkCreateTenantInputDto>();
            services.AddSampleTemplateHandler<BulkCreateQrLocationInputDto>();
            services.AddScoped<IExcelRowBusinessValidator<BulkCreateAppRoleInputDto>, BulkCreateAppRoleNameUniquenessValidator>();
            services.AddScoped<IExcelRowBusinessValidator<BulkCreateTenantInputDto>, BulkCreateTenantNameUniquenessValidator>();
            services.AddValidatorsFromAssembly(typeof(SharedResource).Assembly);
            services.AddTransient(typeof(IValidator<>), typeof(GetByIdQueryValidator<>)); 
            services.AddScoped<IPermissionChangeContext, PermissionChangeContext>();
            return services;
        }

        private static IServiceCollection AddSampleTemplateHandler<TDto>(this IServiceCollection services)
    where TDto : class
    => services.AddTransient<IRequestHandler<GetSampleExcelTemplateQuery<TDto>, Result<ExcelFileDto>>, GetSampleExcelTemplateQueryHandler<TDto>>();
    }
}