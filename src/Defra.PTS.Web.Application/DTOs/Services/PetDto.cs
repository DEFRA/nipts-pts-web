using Defra.PTS.Web.Domain.Enums;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Application.DTOs.Services;

[ExcludeFromCodeCoverage]
public class PetDto
{
    public string Name { get; set; }
    public PetSpecies Species { get; set; }
    public string Breed { get; set; }
    public int BreedId { get; set; }
    public int ColorId { get; set; }
    public PetGender Gender { get; set; }
    public DateTime? DateOfBirth { get; set; }
    public string MicrochipNumber { get; set; }
    public DateTime? MicrochipApplicationDate { get; set; }
}
