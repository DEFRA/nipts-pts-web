using Defra.PTS.Web.CertificateGenerator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using PuppeteerSharp;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Puppeteer;

[ExcludeFromCodeCoverage]

public class PuppeteerBrowserAdapter : IBrowser
{
    private readonly Launcher launcher;
    private readonly IOptions<ConnectOptions> options;
    private readonly IConfiguration configuration;


    public PuppeteerBrowserAdapter(Launcher launcher, IOptions<ConnectOptions> options, IConfiguration configuration)
    {
        ArgumentNullException.ThrowIfNull(launcher);
        ArgumentNullException.ThrowIfNull(options);
        this.launcher = launcher;
        this.options = options;
        this.configuration = configuration;
    }

    public async Task<IPage> NewPageAsync()
    {
        var opt = options.Value;
        var ipAddressUrl = configuration["IpAddressUrl"];
        ArgumentNullException.ThrowIfNull(ipAddressUrl);
        var containerIP = await GetRequestBody(ipAddressUrl);
        var containerURL = string.Format("http://{0}:3000/", containerIP);
        opt.BrowserURL = containerURL;
        var browser = await launcher.ConnectAsync(opt);
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

    public async Task<string> GetRequestBody(string url)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        var bodyText = await response.Content.ReadAsStringAsync();
        return bodyText;
    }
}