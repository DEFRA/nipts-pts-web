using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.UI.Helpers
{
    public interface IBreedHelper
    {
        public Task<List<BreedDto>> GetBreedList(PetSpecies petType);
    }
}
