using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetKeeperNameValidator : AbstractValidator<PetKeeperNameViewModel>
{
    public PetKeeperNameValidator(IStringLocalizer<PetKeeperNameViewModel> localizer)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(x => localizer["Enter your full name"]);

        When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
        {
            RuleFor(x => x.Name).MaximumLength(AppConstants.MaxLength.PetKeeperName).WithMessage($"Name must be {AppConstants.MaxLength.PetKeeperName} characters or less");
            RuleFor(x => x.Name).Matches(AppConstants.RegularExpressions.Name).WithMessage("Enter a name using only letters, hyphens or apostrophes");
        });
    }
}
