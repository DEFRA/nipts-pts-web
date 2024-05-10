using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Helpers;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;
public class PetKeeperPhoneValidator : AbstractValidator<PetKeeperPhoneViewModel>
{
    public PetKeeperPhoneValidator()
    {
        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage(x => $"Enter your phone number");

        When(x => !string.IsNullOrWhiteSpace(x.Phone), () =>
        {
            RuleFor(x => x.Phone)
            .MaximumLength(AppConstants.MaxLength.PetKeeperPhone)
            .WithMessage($"Phone number must be {AppConstants.MaxLength.PetKeeperPhone} characters or less");

            When(x => x.Phone.Length <= AppConstants.MaxLength.PetKeeperPhone, () =>
            {
                RuleFor(x => x.Phone)
                .Must(BeValidUKPhone)
                .WithMessage("Phone number is not valid UK number");
            });
        });
    }

    private static bool BeValidUKPhone(string phone)
    {
        return PhoneNumberHelper.IsValidUKPhoneNumber(phone);
    }
}
