using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Features.Lookups.Queries;
using Defra.PTS.Web.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.UI.Helpers;

public class BreedHelper : IBreedHelper
{
    private readonly IMediator _mediator;
    private readonly IStringLocalizer<SharedResource> localizer;

    public BreedHelper(IMediator mediator, IStringLocalizer<SharedResource> _localizer)
    {
        _mediator = mediator;
        localizer = _localizer;
    }
    public async Task<List<BreedDto>> GetBreedList(PetSpecies petType)
    {
        var response = await _mediator.Send(new GetBreedsQueryRequest(petType));

        var list = response.Breeds.ToList();
    
        foreach (var breed in list)
        {
            breed.BreedName = localizer[breed.BreedName];
        }

        return list;
    }
}
