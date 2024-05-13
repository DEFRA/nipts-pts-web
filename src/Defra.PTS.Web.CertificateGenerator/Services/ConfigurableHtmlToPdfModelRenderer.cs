using Defra.PTS.Web.CertificateGenerator.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Models;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Services;

public sealed class ConfigurableHtmlToPdfModelRenderer<TModel> : HtmlToPdfModelRenderer<TModel>
{
    private readonly IOptions<ConfigurableHtmlToPdfModelRendererOptions<TModel>> options;

    public ConfigurableHtmlToPdfModelRenderer(IHtmlModelRenderer renderer, IHtmlToPdfConverter converter, IOptions<ConfigurableHtmlToPdfModelRendererOptions<TModel>> options)
        : base(renderer, converter)
    {
        ArgumentNullException.ThrowIfNull(options);
        this.options = options;
    }

    protected override Task<Stream> ConvertToPdfAsync(TModel model, HtmlToPdfContext context, CancellationToken cancellationToken)
    {
        context.Margin = options.Value.Margin ?? context.Margin;
        return base.ConvertToPdfAsync(model, context, cancellationToken);
    }

    protected override Task<string> RenderFooterAsync(TModel model, CancellationToken cancellationToken)
    {
        var optionsValue = options.Value;
        return RenderCore(model, optionsValue.FooterTemplate, optionsValue.FooterViewModel, base.RenderFooterAsync, cancellationToken);
    }

    protected override Task<string> RenderHeaderAsync(TModel model, CancellationToken cancellationToken)
    {
        var optionsValue = options.Value;
        return RenderCore(model, optionsValue.HeaderTemplate, optionsValue.HeaderViewModel, base.RenderHeaderAsync, cancellationToken);
    }

    private async Task<string> RenderCore(
        TModel model,
        Func<TModel, string> templateFactory,
        Func<TModel, object> viewModelFactory,
        Func<TModel, CancellationToken, Task<string>> baseImpl,
        CancellationToken cancellationToken)
    {
        var template = templateFactory?.Invoke(model);
        if (template != null)
        {
            return template;
        }

        var viewModel = viewModelFactory?.Invoke(model);
        if (viewModel == null)
        {
            return await baseImpl(model, cancellationToken);
        }

        var result = await htmlRenderer.RenderAsync(viewModel, cancellationToken).ConfigureAwait(false);
        return result.Content;
    }
}