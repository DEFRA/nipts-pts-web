﻿using PuppeteerSharp;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Puppeteer;

[ExcludeFromCodeCoverage]
public class PuppeteerPageAdapter(PuppeteerSharp.IBrowser browser, PuppeteerSharp.IPage page) : IPage
{
    private PuppeteerSharp.IBrowser browser = browser;
    private PuppeteerSharp.IPage page = page;

    private PuppeteerSharp.IBrowser Browser => browser ?? throw new ObjectDisposedException(nameof(PuppeteerBrowserAdapter));
    private PuppeteerSharp.IPage Page => page ?? throw new ObjectDisposedException(nameof(PuppeteerBrowserAdapter));

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
