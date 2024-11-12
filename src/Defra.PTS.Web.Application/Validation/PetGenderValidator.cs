using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation
{
    public class PetGenderValidator : AbstractValidator<PetGenderViewModel>
    {
        public PetGenderValidator(IStringLocalizer<ISharedResource> localizer)
        {
            RuleFor(x => x.Gender)
                .NotEmpty()
                .WithMessage(x => localizer["Tell us if your pet is male or female"]);
        }
    }
}
