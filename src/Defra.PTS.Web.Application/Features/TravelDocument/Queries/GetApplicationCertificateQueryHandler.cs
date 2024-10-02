using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.TravelDocument.Queries;

public class GetApplicationCertificateQueryHandler : IRequestHandler<GetApplicationCertificateQueryRequest, GetApplicationCertificateQueryResponse>
{
    private readonly IApplicationService _applicationService;
    private readonly ILogger<GetApplicationCertificateQueryHandler> _logger;

    public GetApplicationCertificateQueryHandler(IApplicationService applicationService, ILogger<GetApplicationCertificateQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(applicationService);
        ArgumentNullException.ThrowIfNull(logger);

        _applicationService = applicationService;
        _logger = logger;
    }

    public async Task<GetApplicationCertificateQueryResponse> Handle(GetApplicationCertificateQueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = new GetApplicationCertificateQueryResponse
            {
                ApplicationId = request.ApplicationId,
                ApplicationCertificate = await _applicationService.GetApplicationCertificate(request.ApplicationId),
            };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError("{nameof(_applicationService)}: Unable to get application certificate for id {request?.ApplicationId}. Error: {ex.Message}", nameof(_applicationService), request?.ApplicationId, ex.Message);
            throw;
        }
    }

}
