using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using QrAssignment.Application.Common;
using QrAssignment.Application.Common.Excel;
using QrAssignment.Application.Interfaces;
using QrAssignment.Application.Services;
using QrAssignment.Infrastructure.Authentication;
using QrAssignment.Infrastructure.Excel;
using QrAssignment.Infrastructure.Services;
using QrAssignment.Infrastructure.Services.ErrorNotification;
using QrAssignment.Infrastructure.Storage;

namespace QrAssignment.Infrastructure
{

    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<MailSettings>(configuration.GetSection("MailSettings"));

            // Email servis kaydı
            services.AddScoped<IEmailService, EmailService>();
            services.AddScoped<IExcelDataExportGenerator, ExcelDataExportGenerator>();
            services.AddScoped<IExcelSampleTemplateGenerator, ExcelSampleTemplateGenerator>();
            services.AddHttpContextAccessor();
            services.AddScoped<ITenantIdService, TenantIdService>();
            services.AddScoped<IUserContext, UserContext>();
            services.Configure<JwtOptions>(configuration.GetSection("Jwt"));
            services.AddScoped<IJwtProvider, JwtProvider>();
            services.Configure<StorageOptions>(configuration.GetSection(StorageOptions.SectionName));

            var provider = configuration[$"{StorageOptions.SectionName}:Provider"];
            if (string.Equals(provider, "AzureBlob", StringComparison.OrdinalIgnoreCase))
                services.AddSingleton<IFileStorageService, AzureBlobStorageService>();
            else
                services.AddSingleton<IFileStorageService, LocalFileStorageService>();

            services.Configure<ErrorNotificationSettings>(
    configuration.GetSection(ErrorNotificationSettings.SectionName));

            services.AddSingleton<ErrorNotificationChannel>();
            services.AddSingleton<IErrorNotifier>(sp => sp.GetRequiredService<ErrorNotificationChannel>());
            services.AddHostedService<ErrorNotificationWorker>();
            return services;
        }
    }
}
