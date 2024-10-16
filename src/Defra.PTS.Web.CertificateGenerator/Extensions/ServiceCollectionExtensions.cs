using Defra.PTS.Web.CertificateGenerator.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Puppeteer;
using Defra.PTS.Web.CertificateGenerator.RazorHtml;
using Defra.PTS.Web.CertificateGenerator.Services;
using Defra.PTS.Web.CertificateGenerator.ViewModels;
using Defra.PTS.Web.Domain.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using PuppeteerSharp;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.CertificateGenerator.Extensions;

[ExcludeFromCodeCoverage]
public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCertificateServices(this IServiceCollection services, ConfigurationManager configuration)
    {
        services.Configure<PuppeteerSettings>(configuration.GetSection("Puppeteer"));

        services.AddCertificateGeneration();
        services.AddPuppeteerPdfRendering(configuration.GetSection("Puppeteer"));
        services.AddHtmlToPdfRendering<ApplicationCertificateViewModel>();
        services.AddRazorHtmlRendering<ApplicationCertificateViewModel>(opt =>
        {
            opt.ViewName = ApplicationCertificateViewModel.ViewName;
            opt.ViewPath = ApplicationCertificateViewModel.ViewPath;
            opt.GetFileName = m => $"{typeof(ApplicationCertificateViewModel).Name}.html";
        });
        services.AddHtmlToPdfRendering<ApplicationDetailsViewModel>();
        services.AddRazorHtmlRendering<ApplicationDetailsViewModel>(opt =>
        {
            opt.ViewName = ApplicationDetailsViewModel.ViewName;
            opt.ViewPath = ApplicationDetailsViewModel.ViewPath;
            opt.GetFileName = m => $"{typeof(ApplicationDetailsViewModel).Name}.html";
        });

        return services;
    }


    public static IServiceCollection AddCertificateGeneration(this IServiceCollection services)
    {
        services.TryAddTransient<ICertificateGenerator, CompositeCertificateGenerator>();
        services.TryAddTransient<IPdfModelRenderer, CompositePdfModelRenderer>();
        services.TryAddTransient<IHtmlModelRenderer, CompositeHtmlModelRenderer>();

        return services;
    }

    public static IServiceCollection AddPuppeteerPdfRendering(this IServiceCollection services, IConfigurationSection configuration)
    {
        return services.Configure<ConnectOptions>(configuration)
            .AddTransient<Launcher>()
            .AddTransient<IBrowser, PuppeteerBrowserAdapter>()
            .AddTransient<IHtmlToPdfConverter, HtmlToPdfConverter>();
    }

    public static IServiceCollection AddHtmlToPdfRendering<TViewModel>(
        this IServiceCollection services,
        Action<ConfigurableHtmlToPdfModelRendererOptions<TViewModel>> configurePdf = null)
    {
        services.AddCertificateGeneration();

        services.TryAddTransient<ICertificateGenerator<TViewModel>, CertificatePdfGenerator<TViewModel>>();
        services.TryAddTransient<IPdfModelRenderer<TViewModel>, ConfigurableHtmlToPdfModelRenderer<TViewModel>>();

        services.Configure(configurePdf ?? (_ => { }));

        return services;
    }

    public static IServiceCollection AddRazorHtmlRendering<TViewModel>(
    this IServiceCollection services,
    Action<RazorHtmlModelRendererOptions<TViewModel>> configure)
    {
        services.AddHttpContextAccessor();
        services.TryAddTransient<IHtmlModelRenderer<TViewModel>, RazorHtmlModelRenderer<TViewModel>>();
        services.Configure(configure);

        return services;
    }
}