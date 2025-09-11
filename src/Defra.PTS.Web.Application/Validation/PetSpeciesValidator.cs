using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetSpeciesValidator : AbstractValidator<PetSpeciesViewModel>
{
    public PetSpeciesValidator(IStringLocalizer<ISharedResource> localizer)
    {
        RuleFor(x => x.PetSpecies).NotEmpty().WithMessage(x => localizer["Select if your pet is a cat, dog or ferret"]);
    }
}
