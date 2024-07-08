using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;
public class PetFeatureValidator : AbstractValidator<PetFeatureViewModel>
{
    public PetFeatureValidator()
    {
        RuleFor(x => x.HasUniqueFeature).NotEmpty().WithMessage("Tell us if your pet has any significant features");

        When(x => x.HasUniqueFeature == YesNoOptions.Yes, () =>
        {
            RuleFor(x => x.FeatureDescription).NotEmpty().WithMessage("Describe your pet's significant feature");

            When(x => !string.IsNullOrWhiteSpace(x.FeatureDescription), () =>
            {
                RuleFor(x => x.FeatureDescription).MaximumLength(AppConstants.MaxLength.PetFeatureDescription).WithMessage($"Describe your pet's significant feature, using {AppConstants.MaxLength.PetFeatureDescription} characters or less characters");
            });

        });
    }
}
