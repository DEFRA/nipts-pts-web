using Defra.PTS.Web.CertificateGenerator.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Services;

public class CertificatePdfGenerator<TModel> : ICertificateGenerator<TModel>
{
    private readonly IPdfModelRenderer<TModel> renderer;

    public CertificatePdfGenerator(IPdfModelRenderer<TModel> renderer)
    {
        ArgumentNullException.ThrowIfNull(renderer);
        this.renderer = renderer;
    }

    public async Task<CertificateResult> GenerateAsync(TModel certificate, CancellationToken cancellationToken)
    {
        var result = await renderer
            .RenderAsync(certificate, cancellationToken)
            .ConfigureAwait(false);

        return new CertificateResult(result.Name, result.Content, "application/pdf");
    }
}