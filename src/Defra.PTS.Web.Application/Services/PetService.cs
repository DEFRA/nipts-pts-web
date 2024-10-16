using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.DTOs;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Net.Http.Json;

namespace Defra.PTS.Web.Application.Services;

public class PetService : IPetService
{    
    private readonly HttpClient _httpClient;    
    private readonly ILogger<PetService> _logger;    

    public PetService(ILogger<PetService> logger, HttpClient httpClient)
    {
        ArgumentNullException.ThrowIfNull(httpClient);
        ArgumentNullException.ThrowIfNull(logger);        

        _logger = logger;
        _httpClient = httpClient;        
    }

    public async Task<List<BreedDto>> GetBreeds(PetSpecies PetType)
    {        
        try
        {            
            HttpResponseMessage response = await _httpClient.GetAsync("breed/" + (int)PetType);
            response.EnsureSuccessStatusCode();

            string content = await response.Content.ReadAsStringAsync();

            var breeds = JsonConvert.DeserializeObject<List<BreedDto>>(content);


            return breeds;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Error returned from {Message}: {StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }

    public async Task<List<ColourDto>> GetColours(PetSpecies PetType)
    {
        try
        {
            var response = await _httpClient.GetAsync("colour/" + (int)PetType);
            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();

            var result = JsonConvert.DeserializeObject<List<ColourDto>>(content);    

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Exception: Error returned from {Message}: {StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    }

    public async Task<Guid> CreatePet(TravelDocumentViewModel model)
    {
        try
        {
            string apiUrl = _httpClient.BaseAddress + "createpet";

            HttpResponseMessage response = await _httpClient.PostAsJsonAsync(apiUrl, model);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadFromJsonAsync<Guid>();
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Exception: Error returned from {Message}: {StackTrace}", ex.Message, ex.StackTrace);
            throw;
        }
    
    }
}

