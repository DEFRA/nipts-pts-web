﻿using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetMicrochipDateValidator : AbstractValidator<PetMicrochipDateViewModel>
{
    private static readonly string MicrochipError = "Enter a date in the correct format, for example 11 4 2021";
    public PetMicrochipDateValidator(IStringLocalizer<ISharedResource> localizer)
    {
        When(x => x.Day == null, () =>
        {
            RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(localizer[MicrochipError]);
        });

        When(x => x.Month == null, () =>
        {
            When(x => x.Day != null, () =>
            {
                RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer[MicrochipError]);
            }); 
            
            RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(" ");
        });

        When(x => x.Year == null, () =>
        {
            When(x => (x.Day != null && x.Month != null), () =>
            {
                RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
                .NotEmpty().WithMessage(localizer[MicrochipError]);
            }); 
            
            RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(" ");  
        });

        When(x => IsEmptyDate(x), () =>
        {

            RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(localizer[MicrochipError]);

            RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(" ");

            RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(" ");
        });

        When(x => !x.MicrochippedDate.HasValue && x.Day != null && x.Month != null && x.Year != null, () =>
        {
            RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
            .Null().WithMessage(localizer[MicrochipError]);

            RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
            .Null().WithMessage(" ");

            RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
            .Null().WithMessage(" ");
        });

        When(x => x.MicrochippedDate.HasValue && !x.BirthDate.HasValue, () =>
        {
            When(x => !BeTodayOrPastDate(x.MicrochippedDate), () =>
            {
                RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
                .Null().WithMessage(localizer["Enter a date that is in the past"]);

                RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
                .Null().WithMessage(" ");

                RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
                .Null().WithMessage(" ");
            });
            
            var message = localizer["Enter a date that is less than 34 years ago"].Value;
            When(x => !MeetsDateLimits(x.MicrochippedDate, out message), () =>
            {
                RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
                .Null().WithMessage(x => localizer[message]);

                RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
                .Null().WithMessage(" ");

                RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
                .Null().WithMessage(" ");
            });
        });

        When(x => x.BirthDate.HasValue && x.MicrochippedDate.HasValue, () =>
        {
            When(x => !BeTodayOrPastDate(x.MicrochippedDate), () =>
            {
                RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
                .Null().WithMessage(localizer["Enter a date that is in the past"]);

                RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
                .Null().WithMessage(" ");

                RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
                .Null().WithMessage(" ");
            });

            When(x => x.MicrochippedDate <= x.BirthDate, () =>
            {
                RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
                .Null().WithMessage(localizer["Enter a date that is after the pet’s date of birth"]);

                RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
                .Null().WithMessage(" ");

                RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
                .Null().WithMessage(" ");
            });
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

        errorMessage = "Enter a date that is less than 34 years ago";

        var chipDate = date.Value.Date;
        return chipDate >= fromDate;
    }
}
