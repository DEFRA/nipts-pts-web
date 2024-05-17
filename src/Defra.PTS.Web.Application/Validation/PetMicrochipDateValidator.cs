using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;
public class PetMicrochipDateValidator : AbstractValidator<PetMicrochipDateViewModel>
{
    public PetMicrochipDateValidator()
    {
        When(x => IsEmptyDate(x), () =>
        {
            RuleFor(x => x.MicrochippedDate).NotEmpty().WithMessage("Enter the date your pet was microchipped or last scanned");
        });

        When(x => !IsEmptyDate(x), () =>
        {
            RuleFor(x => x.MicrochippedDate).NotEmpty().WithMessage("Microchip date must be a valid date");

            RuleFor(x => x.Day).NotEmpty().WithMessage("Microchip date must indicate a day");
            RuleFor(x => x.Month).NotEmpty().WithMessage("Microchip date must indicate a month");
            RuleFor(x => x.Year).NotEmpty().WithMessage("Microchip date must indicate a year");
        });

        When(x => x.MicrochippedDate.HasValue, () =>
        {
            RuleFor(x => x.MicrochippedDate).Must(BeTodayOrPastDate).WithMessage("Microchip date must must be today or in the past");

            var message = $"Microchip date is not valid";
            RuleFor(x => x.MicrochippedDate).Must((x, e) => MeetsDateLimits(x.MicrochippedDate, out message)).WithMessage(x => message);
        });

        When(x => x.BirthDate.HasValue && x.MicrochippedDate.HasValue, () =>
        {
            RuleFor(x => x.MicrochippedDate).GreaterThan(m => m.BirthDate).WithMessage("Microchip date must be after the date of birth");
        });
    }

    private static bool IsEmptyDate(PetMicrochipDateViewModel model)
    {
        return model.Day.GetValueOrDefault() == 0 && model.Month.GetValueOrDefault() == 0 && model.Year.GetValueOrDefault() == 0;
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

        errorMessage = "The date you entered is too far in the past";

        var chipDate = date.Value.Date;
        return chipDate >= fromDate && chipDate <= toDate;
    }
}
