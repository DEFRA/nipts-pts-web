using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Features.Address.Queries;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetKeeperPostcodeValidator : AbstractValidator<PetKeeperPostcodeViewModel>
{
    private readonly IMediator _mediator;
    public PetKeeperPostcodeValidator(IMediator mediator, IStringLocalizer<SharedResource> localizer)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;

        RuleFor(x => x.Postcode).NotEmpty().WithMessage(x => localizer[$"Enter a postcode"]);
        
        When(x => !string.IsNullOrWhiteSpace(x.Postcode), () =>
        {
            RuleFor(x => x.Postcode).Matches(AppConstants.RegularExpressions.UKPostcode).WithMessage(localizer["Enter a full postcode in the correct format, for example TF7 5AY or TF75AY"]);
            RuleFor(x => x.Postcode).MaximumLength(AppConstants.MaxLength.Postcode).WithMessage(localizer["Enter a full postcode in the correct format, for example TF7 5AY or TF75AY"]);
            RuleFor(x => x.Postcode).Must(BeValidUKPostcode).WithMessage(localizer["Enter a full postcode in the correct format, for example TF7 5AY or TF75AY"]);
        });
    }

    private bool BeValidUKPostcode(string postcode)
    {
        var result = _mediator.Send(new ValidateGreatBritianAddressRequest(postcode)).Result;
        return result;
    }
}
