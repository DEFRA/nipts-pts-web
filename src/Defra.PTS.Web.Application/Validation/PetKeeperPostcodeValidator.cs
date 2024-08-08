using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Features.Address.Queries;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using System;
using System.Text.RegularExpressions;

namespace Defra.PTS.Web.Application.Validation
{
    public class PetKeeperPostcodeValidator : AbstractValidator<PetKeeperPostcodeViewModel>
    {
        private readonly IMediator _mediator;
        private static readonly Regex UkPostcodeRegex = new Regex(AppConstants.RegularExpressions.UKPostcode, RegexOptions.Compiled, TimeSpan.FromMilliseconds(100));

        public PetKeeperPostcodeValidator(IMediator mediator, IStringLocalizer<SharedResource> localizer)
        {
            ArgumentNullException.ThrowIfNull(mediator);
            _mediator = mediator;

            RuleFor(x => x.Postcode)
                .NotEmpty().WithMessage(x => localizer["Enter a postcode"])
                .Custom((postcode, context) =>
                {
                    if (!string.IsNullOrWhiteSpace(postcode))
                    {
                        if (!UkPostcodeRegex.IsMatch(postcode) || postcode.Length > AppConstants.MaxLength.Postcode)
                        {
                            context.AddFailure(localizer["Enter a full postcode in the correct format, for example TF7 5AY or TF75AY"]);
                        }
                        else if (!BeValidUKPostcode(postcode))
                        {
                            context.AddFailure(localizer["Enter a postcode in England, Scotland or Wales"]);
                        }
                    }
                });
        }

        private bool BeValidUKPostcode(string postcode)
        {
            var result = _mediator.Send(new ValidateGreatBritianAddressRequest(postcode)).Result;
            return result;
        }
    }
}
