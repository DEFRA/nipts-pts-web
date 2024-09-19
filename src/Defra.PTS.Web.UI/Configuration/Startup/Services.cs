﻿using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Defra.PTS.Web.Application.Services;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.UI.Configuration.Authentication;
using Defra.PTS.Web.UI.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Components.RenderTree;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Protocols.OpenIdConnect;
using Microsoft.IdentityModel.Tokens;
using NuGet.Configuration;
using Polly;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.Caching;
using static Defra.PTS.Web.UI.Constants.WebAppConstants;

namespace Defra.PTS.Web.UI.Configuration.Startup;

[ExcludeFromCodeCoverage]
public static class Services
{
    public static SecretClient AddKeyVault(this IServiceCollection services, IConfiguration configuration)
    {
        var keyVaultUri = configuration["KeyVaultUri"];
        var client = new SecretClient(new Uri(keyVaultUri), new DefaultAzureCredential());
        return client;
    }
    public static void AddAuthentications(this IServiceCollection services, IConfiguration configuration, SecretClient secretClient)
    {
        services.AddScoped<IHostHelper, HostHelper>();
        services.Configure<SecretsConfiguration>(configuration.GetSection("PTSB2C"));
        string clientSecret = null;
        string clientId = null;
        try
        {
            clientSecret = configuration.GetValue<string>("PTSB2C:PtsB2CTenantClientSecret");
            clientId = configuration.GetValue<string>("PTSB2C:PtsB2CTenantClientId");
        }
        catch { }

        services.AddScoped<ISelectListLocaliser, SelectListLocaliser>();
        services.Configure<CookieConfiguration>(configuration.GetSection("Cookie"));
        services.Configure<CookiePolicyOptions>(options =>
        {
            options.Secure = CookieSecurePolicy.Always;
            options.MinimumSameSitePolicy = SameSiteMode.None;
        });
        services.AddServiceHttpClients(configuration, secretClient);
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = "oidc";
            options.DefaultSignOutScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie(opt => opt.ConfigureCookie(configuration))

        .AddOpenIdConnect("oidc", options =>
        {
            var adB2cSection = configuration.GetSection("AzureAdB2C").Get<OpenIdConnectB2CConfiguration>();
            options.Authority = adB2cSection.Instance;

            options.ClientId = clientId;
            options.ClientSecret = clientSecret;
            options.ResponseType = OpenIdConnectResponseType.Code;
            options.Scope.Add(options.ClientId);
            options.CallbackPath = "/signin-oidc";
            options.SignedOutCallbackPath = "/signout";
            options.MetadataAddress = adB2cSection.MetadataAddress;
            options.SaveTokens = true;
            options.ResponseMode = "form_post";
            options.RemoteAuthenticationTimeout = TimeSpan.FromMinutes(15);
            options.SaveTokens = true;
            options.Events = new OpenIdConnectEvents()
            {
                OnRemoteFailure = (ctx) =>
                {
                    if (ctx.Failure?.Message == "Correlation failed.")
                    {
                        ctx.Response.Redirect("/TravelDocument");
                        ctx.HandleResponse();
                    }

                    return Task.CompletedTask;
                },
            };
            options.Events.OnAuthenticationFailed = _ => Task.CompletedTask;
            options.Events.OnTicketReceived = adB2cSection.HandleTicketReceived;
            options.Events.OnRedirectToIdentityProvider = adB2cSection.HandleRedirectToIdentityProvider;
            options.Events.OnRemoteSignOut = adB2cSection.HandleRemoteSignOut;
            options.Events.OnAccessDenied = ctx =>
            {
                ctx.HandleResponse();
                ctx.Response.StatusCode = StatusCodes.Status401Unauthorized;
                return Task.CompletedTask;
            };

            HttpClientHandler handler = new HttpClientHandler();
            handler.ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator;
            options.BackchannelHttpHandler = handler;
        });

    }

    public static void ConfigureCookie(this CookieAuthenticationOptions options, IConfiguration configuration)
    {
        var config = configuration.GetSection("Cookie").Get<CookieConfiguration>();
        options.Cookie.IsEssential = true;
        options.Cookie.MaxAge = options.ExpireTimeSpan = config.ExpireTimespan;
        options.SlidingExpiration = true;
        options.Cookie.Name = config.Name;
        options.Events.OnValidatePrincipal = context =>
        {
            if (context.Properties.ExpiresUtc is DateTimeOffset expiration)
            {
                context.Response.Headers.Add("X-Auth-Cookie-Expiration", expiration.ToUnixTimeMilliseconds().ToString());
            }

            return Task.CompletedTask;
        };
    }

    public static IServiceCollection AddServiceHttpClients(this IServiceCollection services, IConfiguration configuration, SecretClient secretClient)
    {
        string subscriptionKey = configuration.GetValue<string>("PTSB2C:PtsApimSubscriptionKey");

        services.AddHttpClient<IPetService, PetService>((client) =>
        {
            var petServiceUrl = configuration.GetValue<string>("AppSettings:PetServiceUrl");
            string authToken = MemoryCache.Default[Pages.User.AccessTokenKey] != null ? MemoryCache.Default[Pages.User.AccessTokenKey].ToString() ?? string.Empty : string.Empty;

            client.BaseAddress = new Uri(petServiceUrl);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

        }).ConfigurePrimaryHttpMessageHandler(() =>
            new HttpClientHandler()
            {
                //This must be changed, proper certificate should be added asap by devops team or CCOE.
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            }
        );

        services.AddHttpClient<IApplicationService, ApplicationService>((client) =>
        {
            var ApplicationServiceUrl = configuration.GetValue<string>("AppSettings:ApplicationServiceUrl");
            string authToken = MemoryCache.Default[Pages.User.AccessTokenKey] != null ? MemoryCache.Default[Pages.User.AccessTokenKey].ToString() ?? string.Empty : string.Empty;

            client.BaseAddress = new Uri(ApplicationServiceUrl);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

        }).ConfigurePrimaryHttpMessageHandler(() =>
            new HttpClientHandler()
            {
                //This must be changed, proper certificate should be added asap by devops team or CCOE.
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            }
       );

        services.AddHttpClient<IUserService, UserService>((client) =>
        {
            var userServiceUrl = configuration.GetValue<string>("AppSettings:UserServiceUrl");
            string authToken = MemoryCache.Default[Pages.User.AccessTokenKey] != null ? MemoryCache.Default[Pages.User.AccessTokenKey].ToString() ?? string.Empty : string.Empty;

            client.BaseAddress = new Uri(userServiceUrl);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

        }).ConfigurePrimaryHttpMessageHandler(() =>
            new HttpClientHandler()
            {
                //This must be changed, proper certificate should be added asap by devops team or CCOE.
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            }
       );

        services.AddHttpClient<IDynamicService, DynamicService>((client) =>
        {
            var dynamicsServiceUrl = configuration.GetValue<string>("AppSettings:DynamicServiceUrl");
            string authToken = MemoryCache.Default[Pages.User.AccessTokenKey] != null ? MemoryCache.Default[Pages.User.AccessTokenKey].ToString() ?? string.Empty : string.Empty;

            client.BaseAddress = new Uri(dynamicsServiceUrl);
            client.DefaultRequestHeaders.Add("Authorization", $"Bearer {authToken}");
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

        }).ConfigurePrimaryHttpMessageHandler(() =>
            new HttpClientHandler()
            {
                //This must be changed, proper certificate should be added asap by devops team or CCOE.
                ServerCertificateCustomValidationCallback = (sender, cert, chain, sslPolicyErrors) => { return true; }
            }
      );

        return services;
    }

}

[ExcludeFromCodeCoverage]
public class SessionTimeoutMiddleware
{
    private readonly RequestDelegate _next;

    public SessionTimeoutMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    private void GetCultureRequest(HttpContext context)
    {
        //Check that the previous url is "", which happens when logging into index page
        var referer = context.Request.Headers["Referer"].ToString();
        if (string.IsNullOrEmpty(referer))
        {
            //Check HttpContext Cookies Request from User Controller
            var cultureQuery = context.Request.Query["setLanguage"].ToString();
            if (!string.IsNullOrWhiteSpace(cultureQuery))
            {
                //Get culture from cookie and set language
                var cultureCode = cultureQuery.Split('|').FirstOrDefault(segment => segment.StartsWith("c="))?.Substring(2);
                if (!string.IsNullOrEmpty(cultureCode))
                {
                    var culture = new CultureInfo(cultureCode);
                    CultureInfo.CurrentCulture = culture;
                    CultureInfo.CurrentUICulture = culture;
                }
            }
        }
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Session.IsAvailable)
        {
            // Check if the request is for a static file or for the timeout page to avoid redirection loops
            if (context.Request.Path.StartsWithSegments("/Home/ApplicationTimeout") ||
                context.Request.Path.Value == "/" ||
                context.Request.Path.Value == "/TravelDocument" ||
                context.Request.Path.Value == "/signin-oidc" ||
                context.Request.Path.StartsWithSegments("/css") ||
                context.Request.Path.StartsWithSegments("/js") ||
                context.Request.Path.StartsWithSegments("/images"))
            {
                GetCultureRequest(context);
                await _next(context);
                return;
            }

            if (context.Session.GetString("SessionActive") == null)
            {
                context.Response.Redirect("/Home/ApplicationTimeout");
                return;
            }
        }
        // If session is active, continue to the next middleware in the pipeline
        await _next(context);
    }

}