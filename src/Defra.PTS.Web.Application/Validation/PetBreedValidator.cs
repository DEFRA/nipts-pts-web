using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;
public class PetBreedValidator : AbstractValidator<PetBreedViewModel>
{
    public PetBreedValidator()
    {
        RuleFor(x => x.BreedName).NotEmpty().WithMessage(x =>
        {
            return x.PetTypeNameLowered 
            switch
            {
                "dog" => "Enter your dog's breed, for example, Labrador",
                "cat" => "Enter your cat's breed, for example, Domestic Shorthair, Bengal, Scottish Fold, Ragdoll",
                _ => "Invalid pet type selected",
            };
        });        
        RuleFor(x => x.BreedName).MaximumLength(150).WithMessage(x => $"The breed name must be less than or equal to 150 characters.");
    }
}
