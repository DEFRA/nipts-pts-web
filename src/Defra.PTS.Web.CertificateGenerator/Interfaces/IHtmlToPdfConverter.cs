using Defra.PTS.Web.CertificateGenerator.Models;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Interfaces;

public interface IHtmlToPdfConverter
{
    public Task<Stream> ConvertAsync(HtmlToPdfContext context, CancellationToken cancellationToken = default);
}