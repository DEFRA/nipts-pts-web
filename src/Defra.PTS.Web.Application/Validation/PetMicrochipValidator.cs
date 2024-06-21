using FluentValidation;
using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;

namespace Defra.PTS.Web.Application.Validation
{
    public class PetMicrochipValidator : AbstractValidator<PetMicrochipViewModel>
    {
        public PetMicrochipValidator()
        {
            RuleFor(x => x.Microchipped).NotEmpty().WithMessage("Tell us if your pet is microchipped");

            When(x => x.Microchipped == YesNoOptions.Yes, () =>
            {
                RuleFor(x => x.MicrochipNumber).NotEmpty().WithMessage("Enter your pet's microchip number");

                When(x => !string.IsNullOrWhiteSpace(x.MicrochipNumber), () =>
                {
                    RuleFor(x => x.MicrochipNumber)
                        .Length(AppConstants.MaxLength.PetMicrochipNumber)
                        .WithMessage("Enter your pet’s 15-digit microchip number")
                        .Unless(x => string.IsNullOrEmpty(x.MicrochipNumber) || x.MicrochipNumber.Length == 15);

                    RuleFor(x => x.MicrochipNumber)
                        .Matches(@"^\d{15}$")
                        .WithMessage("Enter a 15-digit number, using only numbers")
                    .When(x => !string.IsNullOrEmpty(x.MicrochipNumber) && x.MicrochipNumber.Length == 15);



                });
            });
        }
    }
}
