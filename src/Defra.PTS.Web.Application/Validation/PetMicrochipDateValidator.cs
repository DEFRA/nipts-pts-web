using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;
public class PetMicrochipDateValidator : AbstractValidator<PetMicrochipDateViewModel>
{
    private static readonly string MicrochipError = "Enter your pet's microchip date in the correct format, for example 11 04 2021";
    public PetMicrochipDateValidator()
    {
        When(x => IsEmptyDate(x), () =>
        {
            RuleFor(x => x.MicrochippedDate).NotEmpty().WithMessage(x => MicrochipError);
        });

        When(x => !IsEmptyDate(x), () =>
        {
            //RuleFor(x => x.MicrochippedDate).NotEmpty().WithMessage(x => MicrochipError);

            RuleFor(x => x.Day).NotEmpty().WithMessage(x => MicrochipError);
            RuleFor(x => x.Month).NotEmpty().WithMessage(x => MicrochipError);
            RuleFor(x => x.Year).NotEmpty().WithMessage(x => MicrochipError);
        });

        When(x => x.MicrochippedDate.HasValue, () =>
        {
            RuleFor(x => x.MicrochippedDate).Must(BeTodayOrPastDate).WithMessage("Enter a date that is in the past");

            var message = "Enter a date that is less than 34 years ago";
            RuleFor(x => x.MicrochippedDate).Must((x, e) => MeetsDateLimits(x.MicrochippedDate, out message)).WithMessage(x => message);
        });

        When(x => x.BirthDate.HasValue && x.MicrochippedDate.HasValue, () =>
        {
            RuleFor(x => x.MicrochippedDate).GreaterThan(m => m.BirthDate).WithMessage("Enter a date that is after the pet’s date of birth");
        });
    }

    private static bool IsEmptyDate(PetMicrochipDateViewModel model)
    {
        _ = int.TryParse(model.Day, out int day);
        _ = int.TryParse(model.Month, out int month);
        _ = int.TryParse(model.Year, out int year);
        return day == 0 && month == 0 && year == 0;
    }

    private static bool BeTodayOrPastDate(DateTime? date)
    {
        return date.GetValueOrDefault().Date <= DateTime.Now.Date;
    }

    private static bool MeetsDateLimits(DateTime? date, out string errorMessage)
    {
        // 1 day after allowed dob
        var fromDate = DateTime.Now.Date.AddYears(-AppConstants.Values.PetMaxAgeInYears).AddDays(1);
        var toDate = DateTime.Now.Date;

        errorMessage = "Enter a date that is less than 34 years ago";

        var chipDate = date.Value.Date;
        return chipDate >= fromDate;
    }
}
