using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Models;
using Defra.PTS.Web.CertificateGenerator.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Certificates.Commands;

public class GenerateCertificatePdfHandler : IRequestHandler<GenerateCertificatePdfRequest, CertificateResult>
{
    private readonly IApplicationService _applicationService;
    private readonly ICertificateGenerator _certificateGenerator;
    private readonly ILogger<GenerateCertificatePdfHandler> _logger;

    public GenerateCertificatePdfHandler(IApplicationService applicationService, ICertificateGenerator certificateGenerator, ILogger<GenerateCertificatePdfHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(applicationService);
        ArgumentNullException.ThrowIfNull(certificateGenerator);
        ArgumentNullException.ThrowIfNull(logger);

        _applicationService = applicationService;
        _certificateGenerator = certificateGenerator;
        _logger = logger;
    }

    public async Task<CertificateResult> Handle(GenerateCertificatePdfRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = new GetApplicationCertificateQueryResponse
            {
                ApplicationId = request.ApplicationId,
                ApplicationCertificate = await _applicationService.GetApplicationCertificate(request.ApplicationId),
            };

            if (!response.ApplicationCertificate.UserId.Equals(request.UserId))
            {
                return null;
            }

            response.ApplicationCertificate.PetKeeperDetails.IsGeneratingPdf = true;
            var model = new ApplicationCertificateViewModel
            {
                Data = response.ApplicationCertificate
            };

            var certificateResult = await _certificateGenerator
                .GenerateAsync(model, CancellationToken.None)
                .ConfigureAwait(false);

            return certificateResult;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "GenerateCertificatePdfHandler: Unable to generate application PDF for ID {0}", request?.ApplicationId);
            return null;
        }
                    
    }

}
