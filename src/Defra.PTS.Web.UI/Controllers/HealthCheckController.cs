using Defra.Trade.Address.V1.ApiClient.Api;
using Defra.Trade.Address.V1.ApiClient.Client;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics.CodeAnalysis;
using static Defra.PTS.Web.UI.Constants.WebAppConstants;

namespace Defra.PTS.Web.UI.Controllers
{
    [ExcludeFromCodeCoverage]
    [Route("/")] // Turns on attribute routing
    public class HealthCheckController : Controller
    {
        private readonly IPlacesApi _placesApi;
        private readonly ILogger<HealthCheckController> _logger;
        public HealthCheckController(
            IPlacesApi placesApi
            , ILogger<HealthCheckController> logger)
        {
            ArgumentNullException.ThrowIfNull(placesApi);

            _placesApi = placesApi;
            _logger = logger;
        }

        [Route("health")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var addresses = await _placesApi.PostCodeLookupAsync(PlacesPostCode.PostCode);
                if(addresses.Count > 0)
                {
                    return new OkResult();
                }
                else
                {
                   return new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
                }               
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, $"Places Api Health Check Error: {ex.Message}");
                // this is for where the api has no values returned for a valid postcode entry
                return new StatusCodeResult(StatusCodes.Status503ServiceUnavailable);
            }
        }
    }
}
