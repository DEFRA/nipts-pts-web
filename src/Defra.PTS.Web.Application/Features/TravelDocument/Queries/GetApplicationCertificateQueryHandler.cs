using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.TravelDocument.Queries;

public class GetApplicationCertificateQueryHandler : IRequestHandler<GetApplicationCertificateQueryRequest, GetApplicationCertificateQueryResponse>
{
    private readonly IApplicationService _applicationService;

    public GetApplicationCertificateQueryHandler(IApplicationService applicationService)
    {
        ArgumentNullException.ThrowIfNull(applicationService);

        _applicationService = applicationService;
    }

    public async Task<GetApplicationCertificateQueryResponse> Handle(GetApplicationCertificateQueryRequest request, CancellationToken cancellationToken)
    {
        var response = new GetApplicationCertificateQueryResponse
        {
            ApplicationId = request.ApplicationId,
            ApplicationCertificate = await _applicationService.GetApplicationCertificate(request.ApplicationId),
        };

        return response;
    }

}
