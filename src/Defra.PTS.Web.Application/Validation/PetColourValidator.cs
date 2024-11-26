using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetColourValidator : AbstractValidator<PetColourViewModel>
{
    public PetColourValidator(IStringLocalizer<ISharedResource> localizer)
    {
        RuleFor(x => x.PetColour).NotEmpty().WithMessage(x => localizer[$"Select the main colour of your {x.PetTypeNameLowered}"]);

        When(x => x.PetColour == x.OtherColourID && x.PetColour > 0, () =>
        {
            RuleFor(x => x.PetColourOther).NotEmpty().WithMessage(x => localizer[$"Describe the main colour of your {x.PetTypeNameLowered}"]);

            RuleFor(x => x.PetColourOther)
                .MaximumLength(AppConstants.MaxLength.PetColourOther)
                .WithMessage(x => localizer[$"Describe the main colour of your {x.PetTypeNameLowered}, using {AppConstants.MaxLength.PetColourOther} characters or less"]);
        });
    }
}
