using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Features.Lookups.Queries;
using Defra.PTS.Web.Domain.DTOs;
using Defra.PTS.Web.Domain.Enums;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.UI.Helpers;

public class SelectListLocaliser(IMediator mediator, IStringLocalizer<ISharedResource> Localizer) : ISelectListLocaliser
{
    private readonly IMediator _mediator = mediator;
    private readonly IStringLocalizer<ISharedResource> localizer = Localizer;

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

    public async Task<List<BreedDto>> GetBreedListWithoutLocalisation(PetSpecies petType)
    {
        var response = await _mediator.Send(new GetBreedsQueryRequest(petType));

        return response.Breeds.ToList();
    }

    public async Task<List<ColourDto>> GetPetColoursList(PetSpecies petType)
    {
        var response = await _mediator.Send(new GetColoursQueryRequest(petType));

        var list = response.Colours.ToList();

        foreach (var colour in list)
        {
            colour.Name = localizer[colour.Name];
        }

        return list;
    }
}
