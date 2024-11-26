using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Lookups.Queries;

public class GetColoursQueryHandler : IRequestHandler<GetColoursQueryRequest, GetColoursQueryResponse>
{
    private readonly IPetService _petService;
    public GetColoursQueryHandler(IPetService petService)
    {
        ArgumentNullException.ThrowIfNull(petService);

        _petService = petService;
    }

    public async Task<GetColoursQueryResponse> Handle(GetColoursQueryRequest request, CancellationToken cancellationToken)
    {
        var response = new GetColoursQueryResponse
        {
            PetType = request.PetType,
            Colours = await _petService.GetColours(request.PetType)
        };

        return response;
    }
}