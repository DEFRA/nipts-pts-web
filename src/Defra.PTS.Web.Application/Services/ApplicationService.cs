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

        _httpClient = httpClient;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApplicationDto> CreateApplication(ApplicationDto application)
    {
        var apiUrl = _httpClient.BaseAddress + "application";

        var response = await _httpClient.PostAsJsonAsync(apiUrl, application);
        if (!response.IsSuccessStatusCode)
        {
            string errorMessage = $"Unable to create application, Status code: {response.StatusCode}";
            _logger.LogError(errorMessage);
            throw new HttpRequestException(errorMessage);
        }

        var content = await response.Content.ReadAsStringAsync();
        var result = JsonConvert.DeserializeObject<ApplicationDto>(content);
        return result;
    }

    public async Task<ApplicationDetailsDto> GetApplicationDetails(Guid applicationId)
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


    public async Task<ApplicationCertificateDto> GetApplicationCertificate(Guid applicationId)
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

    public async Task<List<ApplicationSummaryDto>> GetUserApplications(Guid userId)
    {
        var response = await _httpClient.GetAsync($"Applications/UserApplications/{userId}");
        response.EnsureSuccessStatusCode();

        var content = await response.Content.ReadAsStringAsync();

        var applications = JsonConvert.DeserializeObject<List<ApplicationSummaryDto>>(content);
        return applications;
    }
}
