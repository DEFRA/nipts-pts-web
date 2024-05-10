using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;
public class PetGenderValidator : AbstractValidator<PetGenderViewModel>
{
    public PetGenderValidator()
    {
        RuleFor(x => x.Gender).NotEmpty().WithMessage("Tell us if your pet is male or female");
    }
}
