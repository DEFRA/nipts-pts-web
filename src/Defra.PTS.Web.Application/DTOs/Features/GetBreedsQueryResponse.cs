using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Application.DTOs.Features;

public class GetBreedsQueryResponse
{
    public PetSpecies PetType { get; set; }
    public List<BreedDto> Breeds { get; set; } = new List<BreedDto>();
}
