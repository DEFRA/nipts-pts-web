using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetKeeperAddressValidator : AbstractValidator<PetKeeperAddressViewModel>
{
    public PetKeeperAddressValidator(IStringLocalizer<ISharedResource> localizer)
    {
        RuleFor(x => x.Address).NotEmpty().WithMessage(x => localizer[$"Select your address from the list"]);
    }
}
