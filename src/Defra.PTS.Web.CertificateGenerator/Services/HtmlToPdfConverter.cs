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
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.CertificateGenerator.Services;

[ExcludeFromCodeCoverage]
public class HtmlToPdfConverter : IHtmlToPdfConverter
{
    private readonly ICustomBrowser _browser;
    private readonly ILogger<HtmlToPdfConverter> _logger;

    public HtmlToPdfConverter(ICustomBrowser browser, ILogger<HtmlToPdfConverter> logger)
    {
        ArgumentNullException.ThrowIfNull(browser);

        _browser = browser;
        _logger = logger;
    }

    public async Task<Stream> ConvertAsync(HtmlToPdfContext context, CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(context);

        Puppeteer.IPage page = null;
        
        try
        {
            page = await _browser.NewPageAsync();
        }
        catch (TargetClosedException ex)
        {
            _logger.LogError(ex, "TargetClosedException occurred in HtmlToPdfConverter.ConvertAsync, when trying to invoke _browser.NewPageAsync()");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "General Exception occurred in HtmlToPdfConverter.ConvertAsync, when trying to invoke _browser.NewPageAsync()");
        }
        finally
        {
            if (page != null)
            {
                await page.DisposeAsync();
            }
        }

        
        try
        {
            await page.SetContentAsync(context.Content).ConfigureAwait(false);
        }
        catch (TargetClosedException ex)
        {
            _logger.LogError(ex, "TargetClosedException occurred in HtmlToPdfConverter.ConvertAsync, when trying to invoke page.SetContentAsync(). Content length: {Length}", context.Content?.Length ?? 0);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "General Exception occurred in HtmlToPdfConverter.ConvertAsync when trying to invoke page.SetContentAsync. Content length: {Length}", context.Content?.Length ?? 0);
        }

        try
        {
            var pdfOptions = new PdfOptions
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
                },
                Tagged = true
            };

            return await page.PdfStreamAsync(pdfOptions).ConfigureAwait(false);
        }
        catch (TargetClosedException ex)
        {
            _logger.LogError(ex, "TargetClosedException occurred in HtmlToPdfConverter.ConvertAsync when trying to invoke page.PdfStreamAsync. Header: {Header}, Footer: {Footer}, Margins: {Margins}",
                context.HeaderTemplate != null,
                context.FooterTemplate != null,
                context.Margin);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "General Exception occurred in HtmlToPdfConverter.ConvertAsync when trying to invoke page.PdfStreamAsync. Header: {Header}, Footer: {Footer}, Margins: {Margins}",
                context.HeaderTemplate != null,
                context.FooterTemplate != null,
                context.Margin);
        }

    }
}