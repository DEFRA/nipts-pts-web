using Defra.PTS.Web.CertificateGenerator.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Models;
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
    public HtmlToPdfConverter()
    {
    }

    public async Task<Stream> ConvertAsync(HtmlToPdfContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        using var browserFetcher = new BrowserFetcher();
        await browserFetcher.DownloadAsync();
        var browser = await Puppeteer.LaunchAsync(new LaunchOptions
        {
            Headless = true
        });

        await using var page = await browser.NewPageAsync();
        await page.SetContentAsync(context.Content).ConfigureAwait(false);
        await page.EmulateMediaTypeAsync(MediaType.Print);

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