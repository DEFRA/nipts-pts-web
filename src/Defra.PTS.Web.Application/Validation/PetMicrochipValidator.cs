using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;

using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;

public class PetMicrochipValidator : AbstractValidator<PetMicrochipViewModel>
{
    public PetMicrochipValidator()
    {
        RuleFor(x => x.Microchipped).NotEmpty().WithMessage("Tell us if your pet is microchipped");

        When(x => x.Microchipped == YesNoOptions.Yes, () =>
        {
            RuleFor(x => x.MicrochipNumber).NotEmpty().WithMessage("Enter your pet's microchip number in the correct format");

            When(x => !string.IsNullOrWhiteSpace(x.MicrochipNumber), () =>
            {
                RuleFor(x => x.MicrochipNumber).MinimumLength(AppConstants.MaxLength.PetMicrochipNumber).WithMessage($"Microchip number must be {AppConstants.MaxLength.PetMicrochipNumber} digits long");
                RuleFor(x => x.MicrochipNumber).MaximumLength(AppConstants.MaxLength.PetMicrochipNumber).WithMessage($"Microchip number must be {AppConstants.MaxLength.PetMicrochipNumber} digits long");
                RuleFor(x => x.MicrochipNumber).Matches(AppConstants.RegularExpressions.DigitOnly).WithMessage("Microchip number must be a numeric value");
            });
        });
    }
}
