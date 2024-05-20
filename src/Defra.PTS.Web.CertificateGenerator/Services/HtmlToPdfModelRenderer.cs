using Defra.PTS.Web.CertificateGenerator.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Models;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Services;

[ExcludeFromCodeCoverage]
public class HtmlToPdfModelRenderer<TModel> : IPdfModelRenderer<TModel>
{
    protected readonly IHtmlToPdfConverter converter;
    protected readonly IHtmlModelRenderer htmlRenderer;

    public HtmlToPdfModelRenderer(IHtmlModelRenderer htmlRenderer, IHtmlToPdfConverter converter)
    {
        ArgumentNullException.ThrowIfNull(htmlRenderer);
        this.htmlRenderer = htmlRenderer;
        ArgumentNullException.ThrowIfNull(converter);
        this.converter = converter;
    }

    public async Task<RenderResult<Stream>> RenderAsync(TModel model, CancellationToken cancellationToken)
    {
        var content = await RenderContentAsync(model, cancellationToken).ConfigureAwait(false);

        var context = new HtmlToPdfContext
        {
            Content = content.Content,
            FooterTemplate = await RenderFooterAsync(model, cancellationToken).ConfigureAwait(false),
            HeaderTemplate = await RenderHeaderAsync(model, cancellationToken).ConfigureAwait(false),
        };

        var pdf = await ConvertToPdfAsync(model, context, cancellationToken).ConfigureAwait(false);
        return new RenderResult<Stream>(pdf, EnsurePdfExtension(content.Name));
    }

    protected virtual async Task<Stream> ConvertToPdfAsync(TModel model, HtmlToPdfContext context, CancellationToken cancellationToken) => await converter.ConvertAsync(context, cancellationToken).ConfigureAwait(false);

    protected virtual async Task<RenderResult<string>> RenderContentAsync(TModel model, CancellationToken cancellationToken) => await htmlRenderer.RenderAsync(model, cancellationToken).ConfigureAwait(false);

    protected virtual Task<string> RenderFooterAsync(TModel model, CancellationToken cancellationToken) => Task.FromResult<string>(null);

    protected virtual Task<string> RenderHeaderAsync(TModel model, CancellationToken cancellationToken) => Task.FromResult<string>(null);

    private static string EnsurePdfExtension(string name)
    {
        return name.ToUpperInvariant().Split('.').Last() switch
        {
            "HTML" => $"{name[..^5]}.pdf",
            "PDF" => name,
            _ => $"{name}.pdf"
        };
    }
}