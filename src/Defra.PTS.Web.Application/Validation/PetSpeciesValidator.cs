using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetSpeciesValidator : AbstractValidator<PetSpeciesViewModel>
{
    public PetSpeciesValidator(IStringLocalizer<PetSpeciesViewModel> localizer)
    {
        RuleFor(x => x.PetSpecies).NotEmpty().WithMessage(x => localizer["Select if you are taking a pet dog, cat or ferret"]);
    }
}
