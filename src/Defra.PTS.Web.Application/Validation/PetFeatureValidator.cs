using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetFeatureValidator : AbstractValidator<PetFeatureViewModel>
{
    public PetFeatureValidator(IStringLocalizer<ISharedResource> localizer)
    {
        RuleFor(x => x.HasUniqueFeature).NotEmpty().WithMessage(x => localizer["Tell us if your pet has any significant features"]);

        When(x => x.HasUniqueFeature == YesNoOptions.Yes, () =>
        {
            RuleFor(x => x.FeatureDescription).NotEmpty().WithMessage(x => localizer["Describe your pet's significant feature"]);

            When(x => !string.IsNullOrWhiteSpace(x.FeatureDescription), () =>
            {
                RuleFor(x => x.FeatureDescription).MaximumLength(AppConstants.MaxLength.PetFeatureDescription).WithMessage(localizer[$"Describe your pet's significant feature, using {AppConstants.MaxLength.PetFeatureDescription} characters or less"]);
            });

        });
    }
}
