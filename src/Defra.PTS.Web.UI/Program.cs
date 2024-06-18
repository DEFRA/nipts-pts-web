using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.CertificateGenerator.Extensions;
using Defra.PTS.Web.CertificateGenerator.ViewModels;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Infrastructure.Extensions;
using Defra.PTS.Web.UI.Configuration.Startup;
using Defra.Trade.Common.Api.Infrastructure;
using Defra.Trade.Common.AppConfig;
using Defra.Trade.Common.Security.Authentication.Infrastructure;
using System.Globalization;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
builder.Configuration.ConfigureTradeAppConfiguration(true, "RemosSignUpService:Sentinel");

builder.Configuration.AddUserSecrets(Assembly.GetExecutingAssembly(), true);
#if DEBUG
builder.Configuration.AddJsonFile("appsettings.Development.json", true, true);
#else
           builder.Configuration.AddJsonFile("appsettings.json", true, true);
#endif

builder.Configuration.AddEnvironmentVariables();

_ = builder.Services
    .AddTradeAppConfiguration(builder.Configuration)
    .AddApimAuthentication(builder.Configuration.GetSection(ApimSettings.InternalApim));

// Add services to the container.
_ = builder.Services.AddTradeApi(builder.Configuration);
_ = builder.Services.AddControllersWithViews();
_ = builder.Services.AddRazorPages();
_ = builder.Services.AddHttpClient();
_ = builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
_ = builder.Services.Configure<PtsSettings>(builder.Configuration.GetSection("PTS"));

_ = builder.Services.AddApplicationServices();
_ = builder.Services.AddInfrastructureServices(builder.Configuration);
_ = builder.Services.AddApplicationApis(builder.Configuration);

_ = builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});

_ = builder.Services.AddHttpContextAccessor();
_ = builder.Services.AddMvc().AddSessionStateTempDataProvider();
_ = builder.Services.AddSession();
_ = builder.Services.AddApplicationInsightsTelemetry();

_ = builder.Services.AddCertificateServices(builder.Configuration);

_ = builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
_ = builder.Logging.AddConsole();
_ = builder.Logging.AddDebug();
_ = builder.Logging.AddEventSourceLogger();
_ = builder.Services.AddCors(o => o.AddPolicy("AllowOrigins", builder =>
{
    builder.WithOrigins("*")
           .AllowAnyMethod()
           .AllowAnyHeader();
}));

_ = builder.Services.AddKeyVault(builder.Configuration);
_ = builder.Services.AddAntiforgery();
var secretClient = builder.Services.AddKeyVault(builder.Configuration);
_ = builder.Services.AddSingleton(secretClient);
var useAuth = builder.Configuration.GetValue<bool>("AppSettings:UseAuth");
if (useAuth)
    builder.Services.AddAuthentications(builder.Configuration, secretClient);

var app = builder.Build();
_ = app.Use(async (context, next) =>
{
    context.Response.OnStarting(state =>
    {
        var httpContext = (HttpContext)state;
        httpContext.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
        httpContext.Response.Headers["Pragma"] = "no-cache";
        httpContext.Response.Headers["Expires"] = "0";
        return Task.CompletedTask;
    }, context);

    await next.Invoke();
});
_ = app.UseHttpsRedirection();
_ = app.UseCookiePolicy();
var supportedCultures = new[]
{
    new CultureInfo("en-GB"),
    new CultureInfo("cy")
};

app.UseRequestLocalization(options =>
{
    var questStringCultureProvider = options.RequestCultureProviders[0];
    options.RequestCultureProviders.RemoveAt(0);
    options.RequestCultureProviders.Insert(1, questStringCultureProvider);
    options.DefaultRequestCulture = new Microsoft.AspNetCore.Localization.RequestCulture("en-GB");
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.ApplyCurrentCultureToResponseHeaders = true;
});


_ = app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
});

_ = app.UseSession();
_ = app.UseStatusCodePagesWithReExecute("/Error/HandleError/{0}");
_ = app.UseRouting();
_ = app.UseCors("AllowOrigins");
if (useAuth)
{
    // UseAuthentication() must be before UseAuthorization()
    _ = app.UseAuthentication();
    _ = app.UseAuthorization();
}

_ = app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
