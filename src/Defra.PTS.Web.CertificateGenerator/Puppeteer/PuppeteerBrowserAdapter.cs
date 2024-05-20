using Defra.PTS.Web.CertificateGenerator.Services;
using Microsoft.Extensions.Options;
using PuppeteerSharp;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Puppeteer;

[ExcludeFromCodeCoverage]

public class PuppeteerBrowserAdapter : IBrowser
{
    private readonly Launcher launcher;
    private readonly IOptions<ConnectOptions> options;

    public PuppeteerBrowserAdapter(Launcher launcher, IOptions<ConnectOptions> options)
    {
        ArgumentNullException.ThrowIfNull(launcher);
        ArgumentNullException.ThrowIfNull(options);
        this.launcher = launcher;
        this.options = options;
    }

    public async Task<IPage> NewPageAsync()
    {
        var browser = await launcher.ConnectAsync(options.Value);
        try
        {
            var page = await browser.NewPageAsync();
            return new PuppeteerPageAdapter(browser, page);
        }
        catch
        {
            await browser.DisposeAsync();
            throw;
        }
    }
}