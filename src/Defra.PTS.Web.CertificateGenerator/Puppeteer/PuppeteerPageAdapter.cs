using PuppeteerSharp;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Puppeteer;

[ExcludeFromCodeCoverage]
public class PuppeteerPageAdapter : IPage
{
    private Browser browser;
    private Page page;

    private Browser Browser => browser ?? throw new ObjectDisposedException(nameof(PuppeteerBrowserAdapter));
    private Page Page => page ?? throw new ObjectDisposedException(nameof(PuppeteerBrowserAdapter));

    public PuppeteerPageAdapter(Browser browser, Page page)
    {
        this.browser = browser;
        this.page = page;
    }

    public async ValueTask DisposeAsync()
    {
        await Page.DisposeAsync();
        page = null;
        Browser.Disconnect();
        await Browser.DisposeAsync();
        browser = null;
        GC.SuppressFinalize(this);
    }

    public async Task<Stream> PdfStreamAsync(PdfOptions options) => await Page.PdfStreamAsync(options);

    public async Task SetContentAsync(string content) => await Page.SetContentAsync(content);
}
