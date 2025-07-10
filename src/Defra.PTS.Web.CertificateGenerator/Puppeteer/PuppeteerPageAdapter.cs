using PuppeteerSharp;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.CertificateGenerator.Puppeteer;

[ExcludeFromCodeCoverage]
public class PuppeteerPageAdapter(PuppeteerSharp.IBrowser browser, PuppeteerSharp.IPage page, ILogger<PuppeteerPageAdapter> _logger) : IPage
{
    private PuppeteerSharp.IBrowser browser = browser;
    private PuppeteerSharp.IPage page = page;

    private PuppeteerSharp.IBrowser Browser => browser ?? throw new ObjectDisposedException(nameof(PuppeteerBrowserAdapter));
    private PuppeteerSharp.IPage Page => page ?? throw new ObjectDisposedException(nameof(PuppeteerBrowserAdapter));

    private readonly ILogger<PuppeteerPageAdapter> logger = _logger;

    public async ValueTask DisposeAsync()
    {
        try
        {
            if (Page != null)
            {
                await Page.DisposeAsync();
                page = null;
            }
        }
        catch (TargetClosedException ex)
        {
            logger.LogError(ex, "TargetClosedException occurred in PuppeteerPageAdapter.DisposeAsync while invoking Page.DisposeAsync()");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error disposing Puppeteer page.");
        }

        try
        {
            if (Browser != null)
            {
                Browser.Disconnect();
                await Browser.DisposeAsync();
                browser = null;
            }
        }
        catch (TargetClosedException ex)
        {
            logger.LogError(ex, "TargetClosedException occurred in PuppeteerPageAdapter.DisposeAsync while invoking Browser.DisposeAsync()");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error disposing Puppeteer browser.");
        }

        GC.SuppressFinalize(this);
    }

    public async Task<Stream> PdfStreamAsync(PdfOptions options) => await Page.PdfStreamAsync(options);

    public async Task SetContentAsync(string content) => await Page.SetContentAsync(content);
}
