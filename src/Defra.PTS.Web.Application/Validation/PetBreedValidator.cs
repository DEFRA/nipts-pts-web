using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;
public class PetBreedValidator : AbstractValidator<PetBreedViewModel>
{
    public PetBreedValidator()
    {
        RuleFor(x => x.BreedName).NotEmpty().WithMessage(x => $"Select or enter the breed of your pet");        
        RuleFor(x => x.BreedName).MaximumLength(150).WithMessage(x => $"Enter a breed using 150 characters or less");
    }
}
