using Defra.PTS.Web.CertificateGenerator.Models;
using System.Threading;
using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Interfaces;

public interface ICertificateGenerator
{
    Task<CertificateResult> GenerateAsync<TModel>(TModel certificate, CancellationToken cancellationToken);

    Task<CertificateResult> GenerateAsync(object certificate, CancellationToken cancellationToken);
}

public interface ICertificateGenerator<TModel>
{
    Task<CertificateResult> GenerateAsync(TModel certificate, CancellationToken cancellationToken);
}