﻿using Defra.PTS.Web.CertificateGenerator.Interfaces;
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
        _ = services.Configure<PuppeteerSettings>(configuration.GetSection("Puppeteer"));

        _ = services.AddCertificateGeneration();
        _ = services.AddPuppeteerPdfRendering(configuration.GetSection("Puppeteer"));
        _ = services.AddHtmlToPdfRendering<ApplicationCertificateViewModel>();
        _ = services.AddRazorHtmlRendering<ApplicationCertificateViewModel>(opt =>
        {
            opt.ViewName = ApplicationCertificateViewModel.ViewName;
            opt.ViewPath = ApplicationCertificateViewModel.ViewPath;
            opt.GetFileName = m => $"{typeof(ApplicationCertificateViewModel).Name}.html";
        });
        _ = services.AddHtmlToPdfRendering<ApplicationDetailsViewModel>();
        _ = services.AddRazorHtmlRendering<ApplicationDetailsViewModel>(opt =>
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
            .AddTransient<IHtmlToPdfConverter, HtmlToPdfConverter>();
    }

    public static IServiceCollection AddHtmlToPdfRendering<TViewModel>(
        this IServiceCollection services,
        Action<ConfigurableHtmlToPdfModelRendererOptions<TViewModel>> configurePdf = null)
    {
        _ = services.AddCertificateGeneration();

        services.TryAddTransient<ICertificateGenerator<TViewModel>, CertificatePdfGenerator<TViewModel>>();
        services.TryAddTransient<IPdfModelRenderer<TViewModel>, ConfigurableHtmlToPdfModelRenderer<TViewModel>>();

        _ = services.Configure(configurePdf ?? (_ => { }));

        return services;
    }

    public static IServiceCollection AddRazorHtmlRendering<TViewModel>(
    this IServiceCollection services,
    Action<RazorHtmlModelRendererOptions<TViewModel>> configure)
    {
        _ = services.AddHttpContextAccessor();
        services.TryAddTransient<IHtmlModelRenderer<TViewModel>, RazorHtmlModelRenderer<TViewModel>>();
        _ = services.Configure(configure);

        return services;
    }
}