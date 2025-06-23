using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Features.Address.Queries;
using Defra.PTS.Web.Application.Helpers;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Localization;
using PhoneNumbers;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Application.Validation;
public class DeclarationValidator : AbstractValidator<DeclarationViewModel>
{
    private readonly IMediator _mediator;

    public DeclarationValidator(IMediator mediator)
    {
        
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;

        var phoneNumberUtil = PhoneNumberUtil.GetInstance();

        RuleFor(x => x.AgreedToDeclaration)
    .Must(x => x)
    .WithMessage(System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName == "cy"
        ? "Cytuno â'r datganiad"
        : "Agree to the declaration");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage(x => $"Enter your phone number");

        When(x => !string.IsNullOrWhiteSpace(x.Phone), () =>
        {
            RuleFor(x => x.Phone)
            .Must(phone =>
            {
                try
                {
                    var region = phone.StartsWith('+') ? null : "GB";
                    var parsedNumber = phoneNumberUtil.Parse(phone, region);
                    return phoneNumberUtil.IsValidNumber(parsedNumber);
                }
                catch (NumberParseException)
                {
                    return false;
                }
            })
            .WithMessage("Enter your phone number, like 01632 960 001, 07700 900 982 or +49 30 12345678");
        });
    }

    [ExcludeFromCodeCoverage]
    private bool BeValidUKPostcode(string postcode)
    {
        var result = _mediator.Send(new ValidateGreatBritianAddressRequest(postcode)).Result;
        return result;
    }
}
