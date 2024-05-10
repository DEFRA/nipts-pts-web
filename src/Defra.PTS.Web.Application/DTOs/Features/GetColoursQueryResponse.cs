using Defra.PTS.Web.Domain.DTOs;
using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Application.DTOs.Features;

public class GetColoursQueryResponse
{
    public PetSpecies PetType { get; set; }
    public List<ColourDto> Colours { get; set; } = new List<ColourDto>();
}
