using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Lookups.Queries;

public class GetBreedsQueryHandler : IRequestHandler<GetBreedsQueryRequest, GetBreedsQueryResponse>
{
    private readonly IPetService _petService;

    public GetBreedsQueryHandler(IPetService petService)
    {
        ArgumentNullException.ThrowIfNull(petService);

        _petService = petService;
    }

    public async Task<GetBreedsQueryResponse> Handle(GetBreedsQueryRequest request, CancellationToken cancellationToken)
    {
        var response = new GetBreedsQueryResponse
        {
            PetType = request.PetType,
            Breeds = await _petService.GetBreeds(request.PetType)
        };

        return response;
    }
}