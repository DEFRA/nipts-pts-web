using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;
public class PetKeeperAddressManualValidator : AbstractValidator<PetKeeperAddressManualViewModel>
{
    public PetKeeperAddressManualValidator()
    {
        RuleFor(x => x.AddressLineOne).NotEmpty().WithMessage(x => $"Enter address line 1, typically the building and street");

        When(x => !string.IsNullOrWhiteSpace(x.AddressLineOne), () =>
        {
            RuleFor(x => x.AddressLineOne).MaximumLength(AppConstants.MaxLength.AddressLine).WithMessage($"Address Line One must be {AppConstants.MaxLength.AddressLine} characters or less");
            RuleFor(x => x.AddressLineOne).Matches(AppConstants.RegularExpressions.AddressText).WithMessage("Address Line One is not valid");
        });

        RuleFor(x => x.TownOrCity).NotEmpty().WithMessage(x => $"Enter Town or City");
        When(x => !string.IsNullOrWhiteSpace(x.TownOrCity), () =>
        {
            RuleFor(x => x.TownOrCity).MaximumLength(AppConstants.MaxLength.TownOrCity).WithMessage($"Town Or City must be {AppConstants.MaxLength.TownOrCity} characters or less");
            RuleFor(x => x.TownOrCity).Matches(AppConstants.RegularExpressions.AddressText).WithMessage("Town or City is not valid");
        });

        RuleFor(x => x.Postcode).NotEmpty().WithMessage(x => $"Enter postcode");
        When(x => !string.IsNullOrWhiteSpace(x.Postcode), () =>
        {
            RuleFor(x => x.Postcode).Matches(AppConstants.RegularExpressions.UKPostcode).WithMessage("Enter a full postcode in the correct format, for example TF7 5AY or TF75AY");
            RuleFor(x => x.Postcode).MaximumLength(AppConstants.MaxLength.Postcode).WithMessage($"Postcode must be {AppConstants.MaxLength.Postcode} characters or less");
            When(x => x.PostcodeStartsWithNonGBPrefix(), () =>
            {
                var validPostcode = false;
                RuleFor(x => x.Postcode).Must(x => validPostcode).WithMessage("Enter a postcode in England, Scotland or Wales");
            });
        });

        When(x => !string.IsNullOrWhiteSpace(x.AddressLineTwo), () =>
        {
            RuleFor(x => x.AddressLineTwo).MaximumLength(AppConstants.MaxLength.AddressLine).WithMessage($"Address Line Two must be {AppConstants.MaxLength.AddressLine} characters or less");
            RuleFor(x => x.AddressLineTwo).Matches(AppConstants.RegularExpressions.AddressText).WithMessage("Address Line Two is not valid");
        });

        When(x => !string.IsNullOrWhiteSpace(x.County), () =>
        {
            RuleFor(x => x.County).MaximumLength(AppConstants.MaxLength.County).WithMessage($"County must be {AppConstants.MaxLength.County} characters or less");
            RuleFor(x => x.County).Matches(AppConstants.RegularExpressions.AddressText).WithMessage("County is not valid");
        });
    }
}
