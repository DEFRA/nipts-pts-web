using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetAgeValidator : AbstractValidator<PetAgeViewModel>
{
    private static readonly string BirthDateError = "Enter a date in the correct format, for example 11 04 2021";

    public PetAgeValidator(IStringLocalizer<SharedResource> localizer)
    {
        When(x => IsEmptyDate(x), () =>
        {
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage(localizer[BirthDateError]);
        });

        When(x => !IsEmptyDate(x), () =>
        {
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage(localizer[BirthDateError]);
        });

        When(x => x.BirthDate.HasValue, () =>
        {
            var message = localizer["Enter a date that is less than 34 years ago"].Value;

            RuleFor(x => x.BirthDate).Cascade(CascadeMode.Stop)
                .Must(BePastDate).WithMessage(localizer["Enter a date that is in the past"])
                .Must((x, e) => MeetsDateLimits(x.BirthDate, out message)).WithMessage(x => localizer[message])
                .LessThan(m => m.MicrochippedDate).WithMessage(localizer["Enter a date that is before the pet’s microchip date"]);

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
        var fromDate = DateTime.Now.Date.AddYears(-AppConstants.Values.PetMaxAgeInYears);
        var toDate = DateTime.Now.Date.AddDays(-1);

        errorMessage = "Enter a date that is less than 34 years ago";

        var dob = date.Value.Date;

        return dob >= fromDate && dob <= toDate;
    }
}
