using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Localization;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.IdentityModel.Tokens;
using QrAssignment.Application;
using QrAssignment.Application.Interfaces;
using QrAssignment.Infrastructure;
using QrAssignment.Infrastructure.Localization;
using QrAssignment.Persistance;
using QrAssignment.Persistance.Context;
using QrAssignment.Persistance.Seeding;
using QrAssignment.Persistence.Seeders;
using QrAssignment.Presentation;
using QrAssignment.Presentation.Middlewares;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Sinks.MSSqlServer;
using Serilog.Ui.Core.Extensions;
using Serilog.Ui.MsSqlServerProvider.Extensions;
using Serilog.Ui.Web.Extensions;
using System.Globalization;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.RateLimiting;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

var allowedOrigins = builder.Configuration
    .GetSection("Cors:AllowedOrigins")
    .Get<string[]>() ?? new[] { "http://localhost:4200" };

// -------------------- Serilog UI --------------------
builder.Services.AddSerilogUi(options =>
{
    options.UseSqlServer(sqlOpts =>
    {
        sqlOpts.WithConnectionString(connectionString);
        sqlOpts.WithTable("Logs");
    });
});

// -------------------- CORS (yalnızca Angular) --------------------
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAngularApp", policy =>
    {
        policy.WithOrigins(allowedOrigins)
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddPersistenceServices(builder.Configuration);
builder.Services.AddApplicationServices();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddOpenApi(options =>
{
    options.AddDocumentTransformer((document, context, cancellationToken) =>
    {
        document.Info.Title = "Proje API Başlığı";
        document.Info.Version = "v1";
        document.Info.Description = "Sistemdeki tüm backend servisleri için dokümantasyon.";
        return Task.CompletedTask;
    });
});

// -------------------- Presentation / Controllers --------------------
builder.Services.AddPresentation();
builder.Services.AddControllers()
    .AddApplicationPart(QrAssignment.Presentation.AssemblyReference.Assembly);

// -------------------- Authentication (JWT) --------------------
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.MapInboundClaims = false;

    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:SecretKey"]!)
        ),

        NameClaimType = ClaimTypes.NameIdentifier,
        RoleClaimType = ClaimTypes.Role,

        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorizationBuilder()
    .SetFallbackPolicy(new AuthorizationPolicyBuilder()
        .RequireAuthenticatedUser()
        .Build());

// -------------------- Localization --------------------
builder.Services.AddLocalization();
builder.Services.AddSingleton<JsonLocalizationManager>();
builder.Services.AddSingleton<IStringLocalizerFactory, JsonStringLocalizerFactory>();

var supportedCultures = new[] { "tr-TR", "en-US" };
builder.Services.Configure<RequestLocalizationOptions>(options =>
{
    options.SetDefaultCulture("tr-TR");
    options.AddSupportedCultures(supportedCultures);
    options.AddSupportedUICultures(supportedCultures);
});

// -------------------- Serilog host --------------------
builder.Host.UseSerilog((context, loggerConfig) =>
{
    loggerConfig
        .ReadFrom.Configuration(context.Configuration)
        .WriteTo.Console();

    // SQL sink startup'ta DB'ye bağlanamazsa (serverless DB uykuda olabilir)
    // uygulamayı çökertme; en azından Console'a log basmaya devam et.
    try
    {
        loggerConfig.WriteTo.MSSqlServer(
            connectionString: connectionString,
            sinkOptions: new MSSqlServerSinkOptions
            {
                TableName = "Logs",
                AutoCreateSqlTable = false   // tablo zaten var; startup'ta DB'ye gidip oluşturmaya çalışma
            });
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Serilog SQL sink atlandı: {ex.Message}");
    }
});

builder.Services.AddSingleton<IAppLocalizer, AppLocalizer>();
builder.Services.AddScoped<ILocalizationService, JsonLocalizationManager>();

builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

    options.OnRejected = async (context, token) =>
    {
        context.HttpContext.Response.ContentType = "application/json";
        var errorResponse = "{\"success\": false, \"message\": \"Çok fazla istek attınız. Lütfen bir süre bekleyip tekrar deneyin.\"}";
        await context.HttpContext.Response.WriteAsync(errorResponse, cancellationToken: token);
    };

    options.AddPolicy("IpBasedRateLimit", httpContext =>
        RateLimitPartition.GetFixedWindowLimiter(
            partitionKey: httpContext.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            factory: partition => new FixedWindowRateLimiterOptions
            {
                AutoReplenishment = true,
                PermitLimit = 60,
                QueueLimit = 0,
                Window = TimeSpan.FromMinutes(1)
            }
        ));
});
builder.Services.AddHttpContextAccessor(); 

 
 

var dpSection = builder.Configuration.GetSection(QrAssignment.Persistance.Options.DataProtectionOptions.SectionName);
builder.Services.Configure<QrAssignment.Persistance.Options.DataProtectionOptions>(dpSection);
var dpOptions = dpSection.Get<QrAssignment.Persistance.Options.DataProtectionOptions>() ?? new QrAssignment.Persistance.Options.DataProtectionOptions();

var dataProtection = builder.Services
    .AddDataProtection()
    .SetApplicationName(dpOptions.ApplicationName);

if (builder.Environment.IsDevelopment())
{
    // LOCAL: dosya sistemi + (istersen) PFX sertifika ile sifrele — gorunur test.
    if (!string.IsNullOrWhiteSpace(dpOptions.KeysPath))
    {
        var dir = new DirectoryInfo(dpOptions.KeysPath);
        if (!dir.Exists) dir.Create();
        dataProtection.PersistKeysToFileSystem(dir);
    }

    var cert = LoadDataProtectionCertificate(dpOptions.Certificate); // onceki helper
    if (cert is not null)
        dataProtection.ProtectKeysWithCertificate(cert);
}
else
{
    // AZURE: anahtarlar Blob'da, Key Vault key'i ile sifreli, kimlik = Managed Identity.
    var credential = new Azure.Identity.DefaultAzureCredential();
    
    dataProtection
        .PersistKeysToAzureBlobStorage(
            new Uri(dpOptions.Azure.BlobSasOrUri), credential)   // asagida acikliyorum
        .ProtectKeysWithAzureKeyVault(
            new Uri(dpOptions.Azure.KeyVaultKeyId), credential);
}

static X509Certificate2? LoadDataProtectionCertificate(QrAssignment.Persistance.Options.DataProtectionOptions.CertificateOptions opt)
{
    switch (opt.Source?.Trim().ToLowerInvariant())
    {
        case "store":
            {
                if (string.IsNullOrWhiteSpace(opt.Thumbprint))
                    throw new InvalidOperationException("DataProtection.Certificate.Source=Store icin Thumbprint zorunlu.");

                var storeName = Enum.Parse<StoreName>(opt.StoreName, ignoreCase: true);
                var storeLocation = Enum.Parse<StoreLocation>(opt.StoreLocation, ignoreCase: true);

                using var store = new X509Store(storeName, storeLocation);
                store.Open(OpenFlags.ReadOnly);

                // Temizlik: thumbprint'te bosluk/gizli karakter olabilir.
                var thumb = opt.Thumbprint.Replace(" ", string.Empty).ToUpperInvariant();
                var found = store.Certificates.Find(X509FindType.FindByThumbprint, thumb, validOnly: false);

                if (found.Count == 0)
                    throw new InvalidOperationException($"Sertifika bulunamadi (thumbprint: {thumb}).");

                var c = found[0];
                if (!c.HasPrivateKey)
                    throw new InvalidOperationException("Sertifikanin private key'i yok; anahtarlar cozulemez.");
                return c;
            }

        case "file":
            {
                if (string.IsNullOrWhiteSpace(opt.FilePath))
                    throw new InvalidOperationException("DataProtection.Certificate.Source=File icin FilePath zorunlu.");
                if (!File.Exists(opt.FilePath))
                    throw new InvalidOperationException($"PFX bulunamadi: {opt.FilePath}");

                // .NET 9+: X509CertificateLoader kullan (eski 'new X509Certificate2(path, pwd)' obsolete).
                var c = X509CertificateLoader.LoadPkcs12FromFile(
                    opt.FilePath,
                    opt.Password,
                    X509KeyStorageFlags.MachineKeySet | X509KeyStorageFlags.PersistKeySet);

                if (!c.HasPrivateKey)
                    throw new InvalidOperationException("PFX private key icermiyor; anahtarlar cozulemez.");
                return c;
            }

        default: // "none" / bos
            return null;
    }
}
var app = builder.Build(); 
 
// -------------------- Migration / Seed (opsiyonel) --------------------
// DİKKAT: DB serverless + auto-pause. DB uykudayken bu blok startup'ta
// bağlanmaya çalışır ve DB uyanamazsa uygulama çöker (baştaki crash gibi).
// Açacaksan DB'nin uyanık olduğundan emin ol; aşağıda hata toleransı da var.
// Prod'da her başlangıçta otomatik migration genelde önerilmez — tercihen
// migration'ı deploy dışında bir kez elle çalıştır.
//
if (args.Contains("seed"))
{
    using (var scope = app.Services.CreateScope())
    {
        try
        {
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
            await db.Database.MigrateAsync();
            await new MenuCatalogSeeder(db).SeedAsync();
        }
        catch (Exception ex)
        {
            // Migration/seed başarısız olsa bile uygulamayı ayakta tut.
            Log.Error(ex, "Startup migration/seed sırasında hata oluştu; uygulama devam ediyor.");
        }
    }
    return;
}
app.UseExceptionHandler();

app.UseRouting();
// CORS middleware'i yönlendirmeden (Routing) önce konumlandırıldı.
app.UseCors("AllowAngularApp");

// Localization ayarları
var supportedCulturesInfo = new[] { new CultureInfo("tr-TR"), new CultureInfo("en-US") };
var localizationOptions = new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture("tr-TR"),
    SupportedCultures = supportedCulturesInfo,
    SupportedUICultures = supportedCulturesInfo
};
localizationOptions.RequestCultureProviders.Clear();
app.UseRequestLocalization(localizationOptions);

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi().AllowAnonymous();
    app.MapScalarApiReference().AllowAnonymous();

    app.UseSerilogUi(options =>
    {
        options.WithRoutePrefix("logs");
    });
    await DatabaseSeeder.SeedAsync(app.Services);
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();
app.UseMiddleware<PermissionChangeContextMiddleware>();

app.UseRateLimiter();

app.UseSerilogUi(options =>
{
    options.WithRoutePrefix("logs");
});

app.MapGet("/health", () => Results.Ok(new { status = "Healthy", time = DateTime.UtcNow }))
   .AllowAnonymous();
app.MapControllers()
   .RequireRateLimiting("IpBasedRateLimit");
 
 

await app.RunAsync();