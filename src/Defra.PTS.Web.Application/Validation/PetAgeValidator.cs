using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetAgeValidator : AbstractValidator<PetAgeViewModel>
{
    private static readonly string BirthDateError = "Enter your pet’s date of birth in the correct format, for example, 11 04 2021";

    public PetAgeValidator(IStringLocalizer<ISharedResource> localizer)
    {
        When(x => x.Day == null, () =>
        {
            RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(localizer[BirthDateError]);
        });

        When(x => x.Month == null, () =>
        {
            When(x => x.Day != null, () =>
            {
                RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer[BirthDateError]);
            });

            RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(" ");
        });

        When(x => x.Year == null, () =>
        {
            When(x => (x.Day != null && x.Month != null), () =>
            {
                RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer[BirthDateError]);
            });

            RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(" ");
        });

        When(x => IsEmptyDate(x), () =>
        {
            RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(localizer[BirthDateError]);

            RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(" ");

            RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(" ");
        });

        When(x => !x.BirthDate.HasValue && x.Day != null && x.Month != null && x.Year != null, () =>
        {
            RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
            .Null().WithMessage(localizer[BirthDateError]);

            RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
            .Null().WithMessage(" ");

            RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
            .Null().WithMessage(" ");
        });

        When(x => x.BirthDate.HasValue, () =>
        {
            var message = localizer["The date you entered is too far in the past, check your pet's date of birth is correct"].Value;
            When(x => !MeetsDateLimits(x.BirthDate, out message), () =>
            {
                RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
                .Null().WithMessage(x => localizer[message]);

                RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
                .Null().WithMessage(" ");

                RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
                .Null().WithMessage(" ");
            });

            When(x => !BePastDate(x.BirthDate), () =>
            {
                RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
                .Null().WithMessage(localizer["Enter a date that is in the past"]);

                RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
               .Null().WithMessage(" ");

                RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
                .Null().WithMessage(" ");
            });

            When(x => BePastDate(x.BirthDate) && x.BirthDate >= x.MicrochippedDate, () =>
            {
                RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
                .Null().WithMessage(localizer["Enter a date that is before the pet’s microchip date"]);

                RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
                .Null().WithMessage(" ");

                RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
                .Null().WithMessage(" ");
            });
        });

    }

    private static bool IsEmptyDate(PetAgeViewModel model)
    {
        _ = int.TryParse(model.Day, out int day);
        _ = int.TryParse(model.Month, out int month);
        _ = int.TryParse(model.Year, out int year);
        return day == 0 && month == 0 && year == 0;
    }

    private static bool BePastDate(DateTime? date)
    {
        return date.GetValueOrDefault().Date < DateTime.Now.Date;
    }

    private static bool MeetsDateLimits(DateTime? date, out string errorMessage)
    {
        // 1 day after allowed dob
        var fromDate = DateTime.Now.Date.AddYears(-AppConstants.Values.PetMaxAgeInYears).AddDays(1);

        errorMessage = "The date you entered is too far in the past, check your pet's date of birth is correct";

        var dob = date.Value.Date;
        return dob >= fromDate;
    }
}
