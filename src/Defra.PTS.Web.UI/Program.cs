using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Infrastructure.Extensions;
using Defra.PTS.Web.UI.Configuration.Startup;
using Defra.Trade.Common.Api.Infrastructure;
using Defra.Trade.Common.AppConfig;
using Defra.Trade.Common.Security.Authentication.Infrastructure;
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

builder.Services
    .AddTradeAppConfiguration(builder.Configuration)
    .AddApimAuthentication(builder.Configuration.GetSection(ApimSettings.InternalApim));

// Add services to the container.
builder.Services.AddTradeApi(builder.Configuration);
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.Configure<AppSettings>(builder.Configuration.GetSection("AppSettings"));
builder.Services.Configure<PtsSettings>(builder.Configuration.GetSection("PTS"));

builder.Services.AddApplicationServices();
builder.Services.AddInfrastructureServices(builder.Configuration);
builder.Services.AddApplicationApis(builder.Configuration);

builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssemblyContaining<Program>();
});
builder.Services.AddHttpContextAccessor();
builder.Services.AddMvc().AddSessionStateTempDataProvider();
builder.Services.AddSession();
builder.Services.AddApplicationInsightsTelemetry();
builder.Logging.AddConfiguration(builder.Configuration.GetSection("Logging"));
builder.Logging.AddConsole();
builder.Logging.AddDebug();
builder.Logging.AddEventSourceLogger();
builder.Services.AddCors(o => o.AddPolicy("AllowOrigins", builder =>
{
    builder.WithOrigins("*")
           .AllowAnyMethod()
           .AllowAnyHeader();
}));
builder.Services.AddKeyVault(builder.Configuration);
builder.Services.AddAntiforgery();
var secretClient = builder.Services.AddKeyVault(builder.Configuration);
builder.Services.AddSingleton(secretClient);
var useAuth = builder.Configuration.GetValue<bool>("AppSettings:UseAuth");
if (useAuth)
    builder.Services.AddAuthentications(builder.Configuration, secretClient);

var app = builder.Build();
app.Use(async (context, next) =>
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
app.UseHttpsRedirection();
app.UseCookiePolicy();
app.UseStaticFiles(new StaticFileOptions
{
    ServeUnknownFileTypes = true,
});

app.UseRequestLocalization();
app.UseSession();
app.UseStatusCodePagesWithReExecute("/Error/HandleError/{0}");
app.UseRouting();
app.UseCors("AllowOrigins");
if (useAuth)
{
    // UseAuthentication() must be before UseAuthorization()
    app.UseAuthentication();
    app.UseAuthorization();
}

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
