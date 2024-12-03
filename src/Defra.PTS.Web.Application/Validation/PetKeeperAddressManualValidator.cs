using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using System.Diagnostics.Metrics;
using Microsoft.Extensions.Localization;
using Defra.PTS.Web.Domain;


namespace Defra.PTS.Web.Application.Validation
{
    public class PetKeeperAddressManualValidator : AbstractValidator<PetKeeperAddressManualViewModel>
    {
        public PetKeeperAddressManualValidator(IStringLocalizer<ISharedResource> localizer)
        {
            RuleLevelCascadeMode = CascadeMode.Stop;

            RuleFor(x => x.AddressLineOne).NotEmpty().WithMessage(x => localizer[$"Enter line 1 of your address"]);

            When(x => !string.IsNullOrWhiteSpace(x.AddressLineOne), () =>
            {
                RuleFor(x => x.AddressLineOne)
                    .MaximumLength(AppConstants.MaxLength.AddressLine).WithMessage(x => localizer[$"Enter line 1 of your address using {AppConstants.MaxLength.AddressLine} characters or less"]);
                // Will need to update shared resource max length number if max length changes

            });

            RuleFor(x => x.TownOrCity)
                .NotEmpty().WithMessage(x => localizer[$"Enter your town or city"]);

            When(x => !string.IsNullOrWhiteSpace(x.TownOrCity), () =>
            {
                RuleFor(x => x.TownOrCity)
                    .MaximumLength(AppConstants.MaxLength.TownOrCity).WithMessage(x => localizer[$"Enter your town or city using {AppConstants.MaxLength.TownOrCity} characters or less"]);
            });

            RuleFor(x => x.Postcode)
                .NotEmpty().WithMessage(x => localizer[$"Enter a postcode"]);

            When(x => !string.IsNullOrWhiteSpace(x.Postcode), () =>
            {
                RuleFor(x => x.Postcode).Cascade(CascadeMode.Stop).Matches(AppConstants.RegularExpressions.UKPostcode).WithMessage(x => localizer[$"Enter your full postcode in the correct format, for example TF7 5AY or TF75AY"])
                .MaximumLength(AppConstants.MaxLength.Postcode).WithMessage(x => localizer[$"Enter your full postcode in the correct format, for example TF7 5AY or TF75AY"])
                .Matches($"^(?!BT|JE|GY|IM|bt|je|gy|im).*").WithMessage(x => localizer[$"Enter a postcode in England, Scotland or Wales"]);
            });

            When(x => !string.IsNullOrWhiteSpace(x.AddressLineTwo), () =>
            {
                RuleFor(x => x.AddressLineTwo)
                    .MaximumLength(AppConstants.MaxLength.AddressLine).WithMessage(x => localizer[$"Enter line 2 of your address using {AppConstants.MaxLength.AddressLine} characters or less"]);

            });

            When(x => !string.IsNullOrWhiteSpace(x.County), () =>
            {
                RuleFor(x => x.County)
                    .MaximumLength(AppConstants.MaxLength.County).WithMessage(x => localizer[$"Enter your county using {AppConstants.MaxLength.County} characters or less"]);

            });
        }
    }
}