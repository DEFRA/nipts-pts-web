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
            throw new TargetClosedException("TargetClosedException occurred in HtmlToPdfConverter.ConvertAsync while invoking _browser.NewPageAsync()", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "General Exception occurred in HtmlToPdfConverter.ConvertAsync, when trying to invoke _browser.NewPageAsync()");
            throw new InvalidOperationException("Error occurred in HtmlToPdfConverter.ConvertAsync while invoking _browser.NewPageAsync()", ex);
        }
        finally
        {
            if (page != null)
            {
                await page.DisposeAsync();
            }
            else
            {
                _logger.LogError("Failed to create a new page in HtmlToPdfConverter.ConvertAsync. The page is null.");
            }
        }

        if (page == null)
        {
            _logger.LogError("Failed to create a new page in HtmlToPdfConverter.ConvertAsync. The page is null.");
            return null;
        }

        try
        {
            await page.SetContentAsync(context.Content).ConfigureAwait(false);
        }
        catch (TargetClosedException ex)
        {
            _logger.LogError(ex, "TargetClosedException occurred in HtmlToPdfConverter.ConvertAsync, when trying to invoke page.SetContentAsync(). Content length: {Length}", context.Content?.Length ?? 0);
            throw new TargetClosedException("TargetClosedException occurred in HtmlToPdfConverter.ConvertAsync while invoking page.SetContentAsync()", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "General Exception occurred in HtmlToPdfConverter.ConvertAsync when trying to invoke page.SetContentAsync. Content length: {Length}", context.Content?.Length ?? 0);
            throw new InvalidOperationException("Error occurred in HtmlToPdfConverter.ConvertAsync while invoking page.SetContentAsync()", ex);
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
            throw new TargetClosedException("TargetClosedException occurred in HtmlToPdfConverter.ConvertAsync while invoking page.PdfStreamAsync()", ex);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "General Exception occurred in HtmlToPdfConverter.ConvertAsync when trying to invoke page.PdfStreamAsync. Header: {Header}, Footer: {Footer}, Margins: {Margins}",
                context.HeaderTemplate != null,
                context.FooterTemplate != null,
                context.Margin);
            throw new InvalidOperationException("Error occurred in HtmlToPdfConverter.ConvertAsync while invoking page.PdfStreamAsync()", ex);
        }
        finally
        {
            if (page != null)
            {
                await page.DisposeAsync();
            }
            else
            {
                _logger.LogError("Failed to create a new page in HtmlToPdfConverter.ConvertAsync. The page is null.");
            }
            _logger.LogInformation("HtmlToPdfConverter.ConvertAsync completed successfully and browser disposed.");
        }

    }
}