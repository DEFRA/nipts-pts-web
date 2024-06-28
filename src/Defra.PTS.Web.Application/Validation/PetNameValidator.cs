using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;
public class PetNameValidator : AbstractValidator<PetNameViewModel>
{
    public PetNameValidator()
    {
        RuleFor(x => x.PetName).NotEmpty().WithMessage("Enter your pet's name");

        When(x => !string.IsNullOrWhiteSpace(x.PetName), () =>
        {
            RuleFor(x => x.PetName).MaximumLength(AppConstants.MaxLength.PetName).WithMessage($"Enter your pet's name, using {AppConstants.MaxLength.PetName} characters or less");            
            RuleFor(x => x.PetName).Matches(AppConstants.RegularExpressions.NameWithNumbers).WithMessage("Enter a name using only letters, numbers, hyphens or apostrophes");
        });

    }
}
