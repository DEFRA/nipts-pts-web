using Defra.PTS.Web.Application.DTOs.Services;

namespace Defra.PTS.Web.Application.Services.Interfaces;

public interface IApplicationService
{
    Task<ApplicationDto> CreateApplication(ApplicationDto application);
    Task<ApplicationDetailsDto> GetApplicationDetails(Guid applicationId);
    Task<ApplicationCertificateDto> GetApplicationCertificate(Guid applicationId);
    Task<List<ApplicationSummaryDto>> GetUserApplications(Guid userId);
}
