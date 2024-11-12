using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Helpers;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.TravelDocument.Queries;

public class GetApplicationsQueryHandler : IRequestHandler<GetApplicationsQueryRequest, GetApplicationsQueryResponse>
{
    private readonly IApplicationService _applicationService;
    private readonly ILogger<GetApplicationsQueryHandler> _logger;

    public GetApplicationsQueryHandler(IApplicationService applicationService, ILogger<GetApplicationsQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(applicationService);
        ArgumentNullException.ThrowIfNull(logger);

        _applicationService = applicationService;
        _logger = logger;
    }

    public async Task<GetApplicationsQueryResponse> Handle(GetApplicationsQueryRequest request, CancellationToken cancellationToken)
    {
        var response = new GetApplicationsQueryResponse
        {
            UserId = request.UserId,
            Applications = await _applicationService.GetUserApplications(request.UserId),
        };

        // Map Status
        foreach (var application in response.Applications)
        {
            application.Status = ApplicationHelper.MapStatusToDisplayStatus(application.Status);
        }

        response.Applications = response.Applications
            .Where(x => request.Statuses.Contains(x.Status))
            .ToList();

        return response;
    }
}
