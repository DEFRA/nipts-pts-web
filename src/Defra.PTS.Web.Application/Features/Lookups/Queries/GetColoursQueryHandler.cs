using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Defra.PTS.Web.Application.Features.Lookups.Queries;

public class GetColoursQueryHandler : IRequestHandler<GetColoursQueryRequest, GetColoursQueryResponse>
{
    private readonly IPetService _petService;
    private readonly ILogger<GetColoursQueryHandler> _logger;
    public GetColoursQueryHandler(IPetService petService, ILogger<GetColoursQueryHandler> logger)
    {
        ArgumentNullException.ThrowIfNull(petService);
        ArgumentNullException.ThrowIfNull(logger);

        _petService = petService;
        _logger = logger;
    }

    public async Task<GetColoursQueryResponse> Handle(GetColoursQueryRequest request, CancellationToken cancellationToken)
    {
        try
        {
            var response = new GetColoursQueryResponse
            {
                PetType = request.PetType,
                Colours = await _petService.GetColours(request.PetType)
            };

            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"{nameof(_petService)}: Unable to get list of colours for {request.PetType.GetDescription()}");

            throw;
        }
    }
}