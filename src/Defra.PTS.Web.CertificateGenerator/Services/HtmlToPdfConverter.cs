using Defra.PTS.Web.CertificateGenerator.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Models;
using Defra.PTS.Web.CertificateGenerator.Puppeteer;
using PuppeteerSharp;
using PuppeteerSharp.Media;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Services;

[ExcludeFromCodeCoverage]
public class HtmlToPdfConverter : IHtmlToPdfConverter
{
    private readonly IBrowser _browser;
    public HtmlToPdfConverter(IBrowser browser)
    {
        ArgumentNullException.ThrowIfNull(browser);

        _browser = browser;
    }

    public async Task<Stream> ConvertAsync(HtmlToPdfContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        await using var page = await _browser.NewPageAsync();
        await page.SetContentAsync(context.Content).ConfigureAwait(false);
        return await page.PdfStreamAsync(new PdfOptions
        {
            Format = PaperFormat.A4,
            PrintBackground = true,
            DisplayHeaderFooter = context.FooterTemplate != null || context.HeaderTemplate != null,
            FooterTemplate = context.FooterTemplate,
            HeaderTemplate = context.HeaderTemplate,
            MarginOptions = new MarginOptions
            {
                Bottom = $"{context.Margin.Bottom}px",
                Top = $"{context.Margin.Top}px",
                Left = $"{context.Margin.Left}px",
                Right = $"{context.Margin.Right}px",
            }
        }).ConfigureAwait(false);
    }
}