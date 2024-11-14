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


    public ApplicationService(HttpClient httpClient, IMapper mapper)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(mapper);

        _httpClient = httpClient;
        _mapper = mapper;
    }

    public async Task<ApplicationDto> CreateApplication(ApplicationDto application)
    {
        var apiUrl = _httpClient.BaseAddress + "application";

        var response = await _httpClient.PostAsJsonAsync(apiUrl, application);
        response.EnsureSuccessStatusCode();

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
