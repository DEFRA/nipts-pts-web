using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.TravelDocument.Queries;

public class GetApplicationDetailsQueryHandler : IRequestHandler<GetApplicationDetailsQueryRequest, GetApplicationDetailsQueryResponse>
{
    private readonly IApplicationService _applicationService;
    private readonly ILogger<GetApplicationDetailsQueryHandler> _logger;

    public GetApplicationDetailsQueryHandler(IApplicationService applicationService, ILogger<GetApplicationDetailsQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(applicationService);
        ArgumentNullException.ThrowIfNull(logger);

        _applicationService = applicationService;
        _logger = logger;
    }

    public async Task<GetApplicationDetailsQueryResponse> Handle(GetApplicationDetailsQueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = new GetApplicationDetailsQueryResponse
            {
                ApplicationId = request.ApplicationId,
                ApplicationDetails = await _applicationService.GetApplicationDetails(request.ApplicationId),
            };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(_applicationService)}: Unable to get application details for id {request?.ApplicationId}");
            throw;
        }
    }
}