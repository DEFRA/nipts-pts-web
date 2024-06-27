using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Features.Address.Queries;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using System.Diagnostics.Metrics;

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
            RuleFor(x => x.Postcode)
                .Custom((postcode, context) =>
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(postcode, AppConstants.RegularExpressions.UKPostcode) ||
                        postcode.Length > AppConstants.MaxLength.Postcode)
                    {
                        context.AddFailure(localizer["Enter a full postcode in the correct format, for example TF7 5AY or TF75AY"]);
                    }
                    else if (!BeValidUKPostcode(postcode))
                    {
                        context.AddFailure(localizer["Enter a postcode in England, Scotland or Wales"]);
                    }
                });
        });
    }

    private bool BeValidUKPostcode(string postcode)
    {
        var result = _mediator.Send(new ValidateGreatBritianAddressRequest(postcode)).Result;
        return result;
    }
}