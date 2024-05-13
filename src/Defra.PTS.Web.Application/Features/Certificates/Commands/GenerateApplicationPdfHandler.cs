using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Models;
using Defra.PTS.Web.CertificateGenerator.ViewModels;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Certificates.Commands;

public class GenerateApplicationPdfHandler : IRequestHandler<GenerateApplicationPdfRequest, CertificateResult>
{
    private readonly IApplicationService _applicationService;
    private readonly ICertificateGenerator _certificateGenerator;
    private readonly ILogger<GenerateApplicationPdfHandler> _logger;

    public GenerateApplicationPdfHandler(IApplicationService applicationService, ICertificateGenerator certificateGenerator, ILogger<GenerateApplicationPdfHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(applicationService);
        ArgumentNullException.ThrowIfNull(certificateGenerator);
        ArgumentNullException.ThrowIfNull(logger);

        _applicationService = applicationService;
        _certificateGenerator = certificateGenerator;
        _logger = logger;
    }

    public async Task<CertificateResult> Handle(GenerateApplicationPdfRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = new GetApplicationDetailsQueryResponse
            {
                ApplicationId = request.ApplicationId,
                ApplicationDetails = await _applicationService.GetApplicationDetails(request.ApplicationId),
            };

            var model = new ApplicationDetailsViewModel
            {
                Data = response.ApplicationDetails
            };

            var certificateResult = await _certificateGenerator
                .GenerateAsync(model, CancellationToken.None)
                .ConfigureAwait(false);

            return certificateResult;
        }
        catch (Exception ex)
        {
            var logMessage = $"{nameof(_applicationService)}: Unable to generate application PDF for id {request?.ApplicationId}";
            _logger.LogError(ex, logMessage);
            return null;
        }
    }

}
