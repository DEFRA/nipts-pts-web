using PuppeteerSharp;
using System;
using System.IO;
using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Puppeteer;

public interface IPage : IAsyncDisposable
{
    Task<Stream> PdfStreamAsync(PdfOptions options);

    Task SetContentAsync(string content);
}