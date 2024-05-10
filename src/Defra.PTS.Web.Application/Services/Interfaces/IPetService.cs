using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Domain.DTOs;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;

namespace Defra.PTS.Web.Application.Services.Interfaces;

public interface IPetService
{
    Task<List<BreedDto>> GetBreeds(PetSpecies PetType);
    Task<List<ColourDto>> GetColours(PetSpecies PetType);
    Task<Guid> CreatePet(TravelDocumentViewModel model);
}
