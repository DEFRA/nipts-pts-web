using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Helpers;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;
using PhoneNumbers;

namespace Defra.PTS.Web.Application.Validation;
public class PetKeeperPhoneValidator : AbstractValidator<PetKeeperPhoneViewModel>
{
    public PetKeeperPhoneValidator(IStringLocalizer<ISharedResource> localizer)
    {
        var phoneNumberUtil = PhoneNumberUtil.GetInstance();

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage(localizer["Enter your phone number"]);

        When(x => !string.IsNullOrWhiteSpace(x.Phone), () =>
        {
            RuleFor(x => x.Phone)
             .Must(phone =>
             {
                 try
                 {
                     var region = phone.StartsWith("+") ? null : "GB";
                     var parsedNumber = phoneNumberUtil.Parse(phone, region);
                     return phoneNumberUtil.IsValidNumber(parsedNumber);
                 }
                 catch (NumberParseException)
                 {
                     return false;
                 }
             })
             .WithMessage(localizer["Enter your phone number, like 01632 960 001, 07700 900 982 or +49 30 12345678"]);

        });
    }
}
