using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using System.Diagnostics.Metrics;

namespace Defra.PTS.Web.Application.Validation
{
    public class PetKeeperAddressManualValidator : AbstractValidator<PetKeeperAddressManualViewModel>
    {
        public PetKeeperAddressManualValidator()
        {

            RuleFor(x => x.AddressLineOne).NotEmpty().WithMessage("Enter line 1 of your address");

            When(x => !string.IsNullOrWhiteSpace(x.AddressLineOne), () =>
            {
                RuleFor(x => x.AddressLineOne)
                    .MaximumLength(AppConstants.MaxLength.AddressLine).WithMessage($"Enter line 1 of your address using {AppConstants.MaxLength.AddressLine} characters or less");
            });

            RuleFor(x => x.TownOrCity)
                .NotEmpty().WithMessage("Enter a town or city");

            When(x => !string.IsNullOrWhiteSpace(x.TownOrCity), () =>
            {
                RuleFor(x => x.TownOrCity)
                    .MaximumLength(AppConstants.MaxLength.TownOrCity).WithMessage($"Enter a town or city using {AppConstants.MaxLength.TownOrCity} characters or less");
            });

            RuleFor(x => x.Postcode)
                .NotEmpty().WithMessage("Enter a postcode");

            When(x => !string.IsNullOrWhiteSpace(x.Postcode), () =>
            {
                RuleFor(x => x.Postcode).Matches(AppConstants.RegularExpressions.UKPostcode).WithMessage("Enter a full postcode in the correct format, for example TF7 5AY or TF75AY");
                RuleFor(x => x.Postcode).MaximumLength(AppConstants.MaxLength.Postcode).WithMessage($"Enter a full postcode in the correct format, for example TF7 5AY or TF75AY");

                When(x => x.PostcodeStartsWithNonGBPrefix(), () =>
                {
                    var validPostcode = false;
                    RuleFor(x => x.Postcode).Must(x => validPostcode).WithMessage("Enter a full postcode in the correct format, for example TF7 5AY or TF75AY");
                });
            });

            When(x => !string.IsNullOrWhiteSpace(x.AddressLineTwo), () =>
            {
                RuleFor(x => x.AddressLineTwo)
                    .MaximumLength(AppConstants.MaxLength.AddressLine).WithMessage($"Enter line 2 of your address using {AppConstants.MaxLength.AddressLine} characters or less");

            });

            When(x => !string.IsNullOrWhiteSpace(x.County), () =>
            {
                RuleFor(x => x.County)
                    .MaximumLength(AppConstants.MaxLength.County).WithMessage($"Enter a county using {AppConstants.MaxLength.County} characters or less");

            });
        }
    }
}