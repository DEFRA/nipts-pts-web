using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetKeeperUserDetailsValidator : AbstractValidator<PetKeeperUserDetailsViewModel>
{
    public PetKeeperUserDetailsValidator(IStringLocalizer<PetKeeperUserDetailsViewModel> localizer)
    {
        RuleFor(x => x.UserDetailsAreCorrect).NotEmpty().WithMessage(localizer["Tell us if your details are correct"]);
    }
}
