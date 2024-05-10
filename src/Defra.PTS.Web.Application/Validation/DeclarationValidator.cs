using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Features.Address.Queries;
using Defra.PTS.Web.Application.Helpers;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using MediatR;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Application.Validation;
public class DeclarationValidator : AbstractValidator<DeclarationViewModel>
{
    private readonly IMediator _mediator;
    public DeclarationValidator(IMediator mediator)
    {
        ArgumentNullException.ThrowIfNull(mediator);
        _mediator = mediator;

        RuleFor(x => x.AgreedToAccuracy).Must(x => x).WithMessage("Confirm that you have provided truthful and accurate information");
        RuleFor(x => x.AgreedToPrivacyPolicy).Must(x => x).WithMessage("Agree to Defra's privacy policy");
        RuleFor(x => x.AgreedToDeclaration).Must(x => x).WithMessage("Agree to the declaration");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .WithMessage(x => $"Enter your phone number");

        When(x => !string.IsNullOrWhiteSpace(x.Phone), () =>
        {
            RuleFor(x => x.Phone)
            .MaximumLength(AppConstants.MaxLength.PetKeeperPhone)
            .WithMessage($"Phone number must be {AppConstants.MaxLength.PetKeeperPhone} characters or less");

            When(x => x.Phone.Length <= AppConstants.MaxLength.PetKeeperPhone, () =>
            {
                RuleFor(x => x.Phone)
                .Must(BeValidUKPhone)
                .WithMessage("Phone number is not valid UK number");
            });
        });

        When(x => x.IsManualAddress, () =>
        {
            When(x => ApplicationHelper.PostcodeStartsWithNonGBPrefix(x.Postcode), () =>
            {
                var validPostcode = false;
                RuleFor(x => x.Postcode).Must(x => validPostcode).WithMessage("Enter a postcode in England, Scotland or Wales");
            });
        });

        When(x => !x.IsManualAddress, () =>
        {
            RuleFor(x => x.Postcode).Must(BeValidUKPostcode).WithMessage("Enter a postcode in England, Scotland or Wales");
        });

    }

    [ExcludeFromCodeCoverage]
    private static bool BeValidUKPhone(string phone)
    {
        return PhoneNumberHelper.IsValidUKPhoneNumber(phone);
    }

    private bool BeValidUKPostcode(string postcode)
    {
        var result = _mediator.Send(new ValidateGreatBritianAddressRequest(postcode)).Result;
        return result;
    }

}
