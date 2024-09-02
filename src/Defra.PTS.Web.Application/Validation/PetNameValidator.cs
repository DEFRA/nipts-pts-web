using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetNameValidator : AbstractValidator<PetNameViewModel>
{
    public PetNameValidator(IStringLocalizer<SharedResource> localizer)
    {
        RuleFor(x => x.PetName).NotEmpty().WithMessage(localizer["Enter your pet's name"]);

        When(x => !string.IsNullOrWhiteSpace(x.PetName), () =>
        {
            RuleFor(x => x.PetName).MaximumLength(AppConstants.MaxLength.PetName).WithMessage(localizer[$"Enter your pet's name, using {AppConstants.MaxLength.PetName} characters or less"]);            
        });

    }
}
