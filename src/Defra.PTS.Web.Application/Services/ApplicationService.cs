using AutoMapper;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;
using static System.Net.Mime.MediaTypeNames;

namespace Defra.PTS.Web.Application.Services;

public class ApplicationService : IApplicationService
{
    private readonly HttpClient _httpClient;
    private readonly IMapper _mapper;
    private readonly ILogger<ApplicationService> _logger;


    public ApplicationService(ILogger<ApplicationService> logger, HttpClient httpClient, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(logger);
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(mapper);

        _logger = logger;
        _httpClient = httpClient;
        _mapper = mapper;
    }

    public async Task<ApplicationDto> CreateApplication(ApplicationDto application)
    {
        try
        {
            var apiUrl = _httpClient.BaseAddress + "application";

            var response = await _httpClient.PostAsJsonAsync(apiUrl, application);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<ApplicationDto>(content);
            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Error returned from {Message}: {StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }

    }

    public async Task<ApplicationDetailsDto> GetApplicationDetails(Guid applicationId)
    {
        try
        {
            var applicationDetailsRequesstDto = new ApplicationDetailsRequesstDto
            {
                ApplicationId = applicationId
            };
            var apiUrl = _httpClient.BaseAddress + "Applications/GetApplicationDetails";
            var response = await _httpClient.PostAsJsonAsync(apiUrl, applicationDetailsRequesstDto);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var vwApplication = JsonConvert.DeserializeObject<VwApplication>(content);
            return _mapper.Map<ApplicationDetailsDto>(vwApplication);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Error returned from {Message}: {StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }

    }


    public async Task<ApplicationCertificateDto> GetApplicationCertificate(Guid applicationId)
    {
        try
        {
            var applicationDetailsRequesstDto = new ApplicationDetailsRequesstDto
            {
                ApplicationId = applicationId
            };
            var apiUrl = _httpClient.BaseAddress + "Applications/GetApplicationDetails";
            var response = await _httpClient.PostAsJsonAsync(apiUrl, applicationDetailsRequesstDto);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            var vwApplication = JsonConvert.DeserializeObject<VwApplication>(content);
            return _mapper.Map<ApplicationCertificateDto>(vwApplication);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Error returned from {Message}: {StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }

    public async Task<List<ApplicationSummaryDto>> GetUserApplications(Guid userId)
    {
        try
        {
            var response = await _httpClient.GetAsync($"Applications/UserApplications/{userId}");
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var applications = JsonConvert.DeserializeObject<List<ApplicationSummaryDto>>(content);
            return applications;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Error returned from {Message}: {StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }
}
