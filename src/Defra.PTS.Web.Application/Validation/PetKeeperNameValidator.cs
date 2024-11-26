using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetKeeperNameValidator : AbstractValidator<PetKeeperNameViewModel>
{
    public PetKeeperNameValidator(IStringLocalizer<ISharedResource> localizer)
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage(x => localizer["Enter your full name"]);

        When(x => !string.IsNullOrWhiteSpace(x.Name), () =>
        {
            RuleFor(x => x.Name).MaximumLength(AppConstants.MaxLength.PetKeeperName).WithMessage(x => localizer[$"Enter your full name, using {AppConstants.MaxLength.PetKeeperName} characters or less"]);            
        });
    }
}
