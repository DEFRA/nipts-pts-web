using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetBreedValidator : AbstractValidator<PetBreedViewModel>
{
    public PetBreedValidator(IStringLocalizer<SharedResource> localizer)
    {
        When(x => x.BreedId == 0 && x.BreedName == null, () =>
        {
            RuleFor(x => x.BreedId).GreaterThan(0).WithMessage(x => localizer[$"Select or enter the breed of your pet"]);
        });
        RuleFor(x => x.BreedName).MaximumLength(150).WithMessage(x => localizer[$"Enter a breed using 150 characters or less"]);
    }
}
