using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;
public class PetKeeperUserDetailsValidator : AbstractValidator<PetKeeperUserDetailsViewModel>
{
    public PetKeeperUserDetailsValidator()
    {
        RuleFor(x => x.UserDetailsAreCorrect).NotEmpty().WithMessage("Tell us if your details are correct");
    }
}
