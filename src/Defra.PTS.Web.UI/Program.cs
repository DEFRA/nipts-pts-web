using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.CertificateGenerator.Extensions;
using Defra.PTS.Web.CertificateGenerator.ViewModels;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Infrastructure.Extensions;
using Defra.PTS.Web.UI.Configuration.Startup;
using Defra.Trade.Common.Api.Infrastructure;
using Defra.Trade.Common.AppConfig;
using Defra.Trade.Common.Security.Authentication.Infrastructure;
using Microsoft.AspNetCore.Mvc.Razor;
using Microsoft.Azure.Management.Storage.Fluent.Models;
using System.Globalization;
using System.Reflection;
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddLocalization(opts => { opts.ResourcesPath = "Resources"; });
builder.Services.AddMvc()
        .AddViewLocalization(
            LanguageViewLocationExpanderFormat.Suffix,
            opts => { opts.ResourcesPath = "Resources"; })
        .AddDataAnnotationsLocalization();

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
_ = builder.Services.AddSession(options =>
{
    options.IdleTimeout = TimeSpan.FromMinutes(30);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
_ = builder.Services.AddApplicationInsightsTelemetry();

_ = builder.Services.AddCertificateServices(builder.Configuration);

_ = builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
_ = builder.Logging.AddConsole();
_ = builder.Logging.AddDebug();
_ = builder.Logging.AddEventSourceLogger();

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    // This lambda determines whether user consent for non-essential 
    // cookies is needed for a given request.
    //set to true when GA is implemented
    options.CheckConsentNeeded = context => false;
    options.MinimumSameSitePolicy = SameSiteMode.Strict;
    options.Secure = CookieSecurePolicy.Always;
});

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
    context.Response.Headers.Add("Cache-control", "no-cache, no-store, must-revalidate");
    context.Response.Headers.Add("Pragma", "no-cache");
    context.Response.Headers.Add(
        "Content-Security-Policy",
        $"default-src *; style-src 'self' 'unsafe-inline'; script-src 'self' 'unsafe-inline' https:; img-src 'self' www.googletagmanager.com data:;");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-Xss-Protection", "1; mode=block");
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("Referrer-Policy", "same-origin");
    context.Response.Headers.Add("X-Permitted-Cross-Domain-Policies", "none");
    context.Response.Headers["Expires"] = "0";
    await next();
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
    options.SupportedCultures = supportedCultures;
    options.SupportedUICultures = supportedCultures;
    options.ApplyCurrentCultureToResponseHeaders = true;
});


_ = app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
});

_ = app.UseSession();
_ = app.UseMiddleware<SessionTimeoutMiddleware>();

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
