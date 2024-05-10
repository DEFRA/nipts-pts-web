using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;
public class PetColourValidator : AbstractValidator<PetColourViewModel>
{
    public PetColourValidator()
    {
        RuleFor(x => x.PetColour).NotEmpty().WithMessage(x => $"Select the main colour of your {x.PetTypeNameLowered}");

        When(x => x.PetColour == x.OtherColourID && x.PetColour > 0, () =>
        {
            RuleFor(x => x.PetColourOther).NotEmpty().WithMessage(x => $"Describe the main colour of your {x.PetTypeNameLowered}");

            RuleFor(x => x.PetColourOther).Matches(AppConstants.RegularExpressions.AlphaNumeric).WithMessage("Enter alpha numeric characters only");
            
            RuleFor(x => x.PetColourOther).MaximumLength(AppConstants.MaxLength.PetColourOther).WithMessage($"Colour description must be {AppConstants.MaxLength.PetColourOther} characters or less");
        });
    }
}
