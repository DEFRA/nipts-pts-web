using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Features.Address.Queries;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetKeeperPostcodeValidator : AbstractValidator<PetKeeperPostcodeViewModel>
{
    private readonly IMediator _mediator;
    public PetKeeperPostcodeValidator(IMediator mediator, IStringLocalizer<PetKeeperPostcodeViewModel> localizer)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;

        RuleFor(x => x.Postcode).NotEmpty().WithMessage(x => localizer[$"Enter your postcode"]);
        
        When(x => !string.IsNullOrWhiteSpace(x.Postcode), () =>
        {
            RuleFor(x => x.Postcode).Matches(AppConstants.RegularExpressions.UKPostcode).WithMessage(localizer["Postcode is not valid"]);
            RuleFor(x => x.Postcode).MaximumLength(AppConstants.MaxLength.Postcode).WithMessage($"Postcode must be {AppConstants.MaxLength.Postcode} characters or less");
            
            RuleFor(x => x.Postcode).Must(BeValidUKPostcode).WithMessage(localizer["Enter a postcode in England, Scotland or Wales"]);
        });
    }

    private bool BeValidUKPostcode(string postcode)
    {
        var result = _mediator.Send(new ValidateGreatBritianAddressRequest(postcode)).Result;
        return result;
    }
}
