using Defra.PTS.Web.CertificateGenerator.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using PuppeteerSharp;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Puppeteer;

[ExcludeFromCodeCoverage]

public class PuppeteerBrowserAdapter : ICustomBrowser
{
    private readonly Launcher launcher;
    private readonly IOptions<ConnectOptions> options;
    private readonly IConfiguration configuration;
    private readonly ILogger<PuppeteerBrowserAdapter> _logger;
    private readonly ILogger<PuppeteerPageAdapter> _pageLogger;


    public PuppeteerBrowserAdapter(
        Launcher launcher,
        IOptions<ConnectOptions> options,
        IConfiguration configuration,
        ILogger<PuppeteerBrowserAdapter> logger,
        ILogger<PuppeteerPageAdapter> pageLogger)
    {
        ArgumentNullException.ThrowIfNull(launcher);
        ArgumentNullException.ThrowIfNull(options);
        this.launcher = launcher;
        this.options = options;
        this.configuration = configuration;
        _logger = logger;
        _pageLogger = pageLogger;
    }

    public async Task<IPage> NewPageAsync()
    {
        var opt = options.Value;
        await GetContainerIp(opt);
        var browser = await launcher.ConnectAsync(opt);
        try
        {
            var page = await browser.NewPageAsync();
            _logger.LogInformation("Attempting to invoke browser.NewPageAsync() in PuppeteerBrowserAdapter.NewPageAsync with options: {Options}", opt);
            return new PuppeteerPageAdapter(browser, page, _pageLogger);
        }
        catch (TargetClosedException ex)
        {
            _logger.LogError(ex, "TargetClosedException occurred in PuppeteerBrowserAdapter.NewPageAsync, options value: {Options}", opt);
            await browser.DisposeAsync();
            throw new TargetClosedException($"TargetClosedException in PuppeteerBrowserAdapter.NewPageAsync with options: {opt}", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An exception occurred in PuppeteerBrowserAdapter.NewPageAsync, invoking browser.DisposeAsync, options value: {Options}", opt);
            await browser.DisposeAsync();
            throw new InvalidOperationException($"An exception occurred in PuppeteerBrowserAdapter.NewPageAsync with options: {opt}", ex);
        }
        
    }

    private async Task GetContainerIp(ConnectOptions opt)
    {
        var useIpAddressUrl = configuration.GetValue<bool>("AppSettings:UseIpAddressUrl");
        if (useIpAddressUrl)
        {
            var ipAddressUrl = configuration.GetValue<string>("AppSettings:IpAddressUrl");
            ArgumentNullException.ThrowIfNull(ipAddressUrl);
            var containerIP = await GetRequestBody(ipAddressUrl);
            var containerURL = string.Format("http://{0}:3000/", containerIP);
            opt.BrowserURL = containerURL;
        }
    }

    public static async Task<string> GetRequestBody(string url)
    {
        using var client = new HttpClient();
        var response = await client.GetAsync(url);
        var bodyText = await response.Content.ReadAsStringAsync();
        return bodyText;
    }
}