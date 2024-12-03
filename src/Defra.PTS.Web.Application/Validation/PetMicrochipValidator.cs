using FluentValidation;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.Domain;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation
{
    public class PetMicrochipValidator : AbstractValidator<PetMicrochipViewModel>
    {
        public PetMicrochipValidator(IStringLocalizer<ISharedResource> localizer)
        {
            RuleFor(x => x.Microchipped).NotEmpty().WithMessage(localizer[@"Select if your pet is microchipped"]);

            When(x => x.Microchipped == YesNoOptions.Yes, () =>
            {
                RuleFor(x => x.MicrochipNumber).NotEmpty().WithMessage(localizer[@"Enter your pet’s 15-digit microchip number"]);

                When(x => !string.IsNullOrWhiteSpace(x.MicrochipNumber), () =>
                {
                    RuleFor(x => x.MicrochipNumber)
                        .Custom((microchipNumber, context) =>
                        {
                            if (IsAllDigits(microchipNumber) && microchipNumber.Length != 15)
                            {
                                context.AddFailure("MicrochipNumber", localizer[@"Enter your pet’s 15-digit microchip number"]);
                            }
                            else if (!IsAllDigits(microchipNumber) || microchipNumber.Length != 15)
                            {
                                context.AddFailure("MicrochipNumber", localizer[@"Enter a 15-digit number, using only numbers"]);
                            }
                        });
                });
            });
        }

        private static bool IsAllDigits(string microchipNumber)
        {
            return microchipNumber.All(char.IsDigit);
        }
    }
}
