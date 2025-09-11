using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Helpers;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetKeeperPhoneValidator : AbstractValidator<PetKeeperPhoneViewModel>
{
    public PetKeeperPhoneValidator(IStringLocalizer<ISharedResource> localizer)
    {

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage(localizer["Enter your phone number, like 01632 960 001 or 07700 900 982"]);

        When(x => !string.IsNullOrWhiteSpace(x.Phone), () =>
        {
            RuleFor(x => x.Phone)
            .MaximumLength(AppConstants.MaxLength.PetKeeperPhone)
            .WithMessage(localizer["Enter your phone number, like 01632 960 001 or 07700 900 982"]);

            When(x => x.Phone.Length <= AppConstants.MaxLength.PetKeeperPhone, () =>
            {
                RuleFor(x => x.Phone)
                .Matches(AppConstants.RegularExpressions.UKPhone)
                .WithMessage(localizer["Enter your phone number, like 01632 960 001 or 07700 900 982"]);
            });
        });
    }
}
