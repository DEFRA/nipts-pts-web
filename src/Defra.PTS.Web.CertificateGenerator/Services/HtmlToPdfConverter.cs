using Defra.PTS.Web.CertificateGenerator.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Models;
using Defra.PTS.Web.Domain.Models;
using Microsoft.Extensions.Options;
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
    private readonly PuppeteerSettings _settings;
    public HtmlToPdfConverter(IOptions<PuppeteerSettings> settingOptions)
    {
        ArgumentNullException.ThrowIfNull(settingOptions);

        _settings = settingOptions.Value;
    }

    public async Task<Stream> ConvertAsync(HtmlToPdfContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        var browser = await GetBrowser(_settings.BrowserURL);

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

    [ExcludeFromCodeCoverage]
    private async Task<IBrowser> GetBrowser(string browserUrl = "")
    {

        // Where Puppeteer can download chromium
        if (string.IsNullOrWhiteSpace(browserUrl)) 
        {
            // Download the Chromium revision if it does not already exist
            using var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();

            return await Puppeteer.LaunchAsync(new LaunchOptions
            {
                Headless = true,
            });
        }

        // Azure doesn't allow chromium download, use a url
        return await Puppeteer.LaunchAsync(new LaunchOptions
        {
            ExecutablePath = browserUrl
        });
    }
}