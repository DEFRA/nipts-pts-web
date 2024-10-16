using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Net.Http.Json;
namespace Defra.PTS.Web.Application.Services;

public class DynamicService : IDynamicService
{
    private readonly HttpClient _httpClient;
    private readonly ILogger<DynamicService> _logger;

    public DynamicService(
            IOptions<AppSettings> appSettings
        , HttpClient httpClient
        , ILogger<DynamicService> logger)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(appSettings);
        ArgumentNullException.ThrowIfNull(logger);

        _httpClient = httpClient;
        _logger = logger;
        _httpClient.BaseAddress = new Uri(appSettings.Value.DynamicServiceUrl);
    }

    public async Task AddAddressAsync(User user)
    {

        string apiUrl = _httpClient.BaseAddress + "fetchupdateaddress";
        var postData = new
        {
            contactId = user.ContactId,
            address = new Address()
        };

        HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, postData);
        if (response.IsSuccessStatusCode)
        {
            Console.WriteLine("Address added");
        }
        else
        {
            Console.WriteLine("Error: " + response.StatusCode);
        }
    }

    public async Task AddApplicationToQueueAsync(ApplicationSubmittedMessage application)
    {
        try
        {
            string apiUrl = _httpClient.BaseAddress + "writetoqueue";

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, application);
            if (response.IsSuccessStatusCode)
            {
                _logger.LogInformation("Message Added to Queue Successfully : {IsSuccessStatusCode}", response.IsSuccessStatusCode);
            }
            else
            {
                _logger.LogError("Message post Failed: {statusCode}", response.StatusCode);                
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "AddApplicationToQueueAsync: Message {Message} : StackTrace {StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }        
    }
}
