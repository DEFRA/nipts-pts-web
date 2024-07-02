﻿using Defra.PTS.Web.Application.Constants;
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
            RuleFor(x => x.MicrochippedDate).NotEmpty().WithMessage("Enter your pet's microchip date in the correct format, for example 11 04 2021");
        });

        When(x => !IsEmptyDate(x), () =>
        {
            RuleFor(x => x.MicrochippedDate).NotEmpty().WithMessage("Enter your pet's microchip date in the correct format, for example 11 04 2021");

            RuleFor(x => x.Day).NotEmpty().WithMessage("Enter your pet's microchip date in the correct format, for example 11 04 2021");
            RuleFor(x => x.Month).NotEmpty().WithMessage("Enter your pet's microchip date in the correct format, for example 11 04 2021");
            RuleFor(x => x.Year).NotEmpty().WithMessage("Enter your pet's microchip date in the correct format, for example 11 04 2021");
        });

        When(x => IsEmptyDate(x) && x.Day.HasValue && x.Month.HasValue && x.Year.HasValue, () =>
        {
            RuleFor(x => x.MicrochippedDate).NotEmpty().WithMessage("Enter your pet's microchip date in the correct format, for example 11 04 2021");
        });

        When(x => x.MicrochippedDate.HasValue, () =>
        {
            RuleFor(x => x.MicrochippedDate).Must(BeTodayOrPastDate).WithMessage("Enter a date that is in the past");

            var message = $"Microchip date is not valid";
            RuleFor(x => x.MicrochippedDate).Must((x, e) => MeetsDateLimits(x.MicrochippedDate, out message)).WithMessage(x => message);
        });

        When(x => x.BirthDate.HasValue && x.MicrochippedDate.HasValue, () =>
        {
            RuleFor(x => x.MicrochippedDate).GreaterThan(m => m.BirthDate).WithMessage("Enter a date that is after the pet’s date of birth");
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
        return chipDate >= fromDate;
    }
}
