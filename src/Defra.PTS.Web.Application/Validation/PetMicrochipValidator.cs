using FluentValidation;
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
                    RuleFor(x => x.MicrochipNumber)
                        .Custom((microchipNumber, context) =>
                        {
                            if (IsAllDigits(microchipNumber) && microchipNumber.Length != 15)
                            {
                                context.AddFailure("MicrochipNumber", "Enter your pet’s 15-digit microchip number");
                            }
                            else if (!IsAllDigits(microchipNumber))
                            {
                                context.AddFailure("MicrochipNumber", "Enter a 15-digit number, using only numbers");
                            }
                        });
                });
            });
        }

        private bool IsAllDigits(string microchipNumber)
        {
            return microchipNumber.All(char.IsDigit);
        }
}