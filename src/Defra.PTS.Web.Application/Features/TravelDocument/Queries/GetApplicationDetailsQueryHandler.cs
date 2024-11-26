using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.TravelDocument.Queries;

public class GetApplicationDetailsQueryHandler : IRequestHandler<GetApplicationDetailsQueryRequest, GetApplicationDetailsQueryResponse>
{
    private readonly IApplicationService _applicationService;

    public GetApplicationDetailsQueryHandler(IApplicationService applicationService)
    {
        ArgumentNullException.ThrowIfNull(applicationService);

        _applicationService = applicationService;
    }

    public async Task<GetApplicationDetailsQueryResponse> Handle(GetApplicationDetailsQueryRequest request, CancellationToken cancellationToken)
    {
        var response = new GetApplicationDetailsQueryResponse
        {
            ApplicationId = request.ApplicationId,
            ApplicationDetails = await _applicationService.GetApplicationDetails(request.ApplicationId),
        };

        return response;
    }
}