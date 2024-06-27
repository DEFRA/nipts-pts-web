using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;

namespace Defra.PTS.Web.Application.Validation;
public class PetAgeValidator : AbstractValidator<PetAgeViewModel>
{
    public PetAgeValidator()
    {
        When(x => IsEmptyDate(x), () =>
        {
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Enter your pet's date of birth in the correct format, for example 11 04 2021");
        });

        When(x => !IsEmptyDate(x), () =>
        {
            RuleFor(x => x.BirthDate).NotEmpty().WithMessage("Enter your pet's date of birth in the correct format, for example 11 04 2021");

            RuleFor(x => x.Day).NotEmpty().WithMessage("Date of birth must indicate a day");
            RuleFor(x => x.Month).NotEmpty().WithMessage("Date of birth must indicate a month");
            RuleFor(x => x.Year).NotEmpty().WithMessage("Date of birth must indicate a year");
        });

        When(x => x.BirthDate.HasValue, () =>
        {
            RuleFor(x => x.BirthDate).Must(BePastDate).WithMessage("Enter a date that is in the past");
            RuleFor(x => x.BirthDate).LessThan(m => m.MicrochippedDate).WithMessage("Enter a date that is before the pet’s microchip date");

            var message = $"Enter your pet's date of birth in the correct format, for example 11 04 2021";
            RuleFor(x => x.BirthDate).Must((x, e) => MeetsDateLimits(x.BirthDate, out message)).WithMessage(x => message);
        });

    }

    private static bool IsEmptyDate(PetAgeViewModel model)
    {
        return model.Day.GetValueOrDefault() == 0 && model.Month.GetValueOrDefault() == 0 && model.Year.GetValueOrDefault() == 0;
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
