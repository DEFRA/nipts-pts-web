using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Lookups.Queries;

public class GetBreedsQueryHandler : IRequestHandler<GetBreedsQueryRequest, GetBreedsQueryResponse>
{
    private readonly IPetService _petService;
    private readonly ILogger<GetBreedsQueryHandler> _logger;

    public GetBreedsQueryHandler(IPetService petService, ILogger<GetBreedsQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(petService);
        ArgumentNullException.ThrowIfNull(logger);

        _petService = petService;
        _logger = logger;
    }

    public async Task<GetBreedsQueryResponse> Handle(GetBreedsQueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = new GetBreedsQueryResponse
            {
                PetType = request.PetType,
                Breeds = await _petService.GetBreeds(request.PetType)
            };

            return response;
        }
        catch (Exception ex) 
        {
            _logger.LogError(ex, "{petService}: Unable to get list of breeds for {PetType}", nameof(_petService), request.PetType.GetDescription());
            throw;
        }
    }
}