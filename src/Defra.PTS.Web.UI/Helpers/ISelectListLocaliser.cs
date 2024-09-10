using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Domain.DTOs;
using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.UI.Helpers
{
    public interface ISelectListLocaliser
    {
        public Task<List<BreedDto>> GetBreedList(PetSpecies petType);
        public Task<List<ColourDto>> GetPetColoursList(PetSpecies petType);
    }
}
