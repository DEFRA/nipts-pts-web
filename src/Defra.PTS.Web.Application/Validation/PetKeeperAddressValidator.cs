using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;
public class PetKeeperAddressValidator : AbstractValidator<PetKeeperAddressViewModel>
{
    public PetKeeperAddressValidator()
    {
        RuleFor(x => x.Address).NotEmpty().WithMessage(x => $"Select your address");
        When(x => x.PostcodeStartsWithNonGBPrefix(), () =>
        {
            var validPostcode = false;
            RuleFor(x => x.Postcode).Must(x => validPostcode).WithMessage("Enter a postcode in England, Scotland or Wales");
        });
    }
}
