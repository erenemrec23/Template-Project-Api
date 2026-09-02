using Microsoft.Extensions.DependencyInjection;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Services;
using QrAssignment.Presentation.Middlewares;
using QrAssignment.Presentation.Services;

namespace QrAssignment.Presentation;

public static class DependencyInjection
{
    public static IServiceCollection AddPresentation(this IServiceCollection services)
    { 
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
        services.AddScoped<ICurrentUserService, CurrentUserService>();
       
        return services;
    }
}