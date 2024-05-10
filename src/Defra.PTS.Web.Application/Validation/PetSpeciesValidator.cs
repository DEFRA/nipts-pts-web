using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;
public class PetSpeciesValidator : AbstractValidator<PetSpeciesViewModel>
{
    public PetSpeciesValidator()
    {
        RuleFor(x => x.PetSpecies).NotEmpty().WithMessage("Select if you are taking a pet dog, cat or ferret");
    }
}
