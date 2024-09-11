using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation;
using Microsoft.Extensions.Localization;

namespace Defra.PTS.Web.Application.Validation;
public class PetMicrochipDateValidator : AbstractValidator<PetMicrochipDateViewModel>
{
    private static readonly string MicrochipError = "Enter a date in the correct format, for example 11 04 2021";
    public PetMicrochipDateValidator(IStringLocalizer<SharedResource> localizer)
    {
        //When(x => IsEmptyDate(x), () =>
        //{
        //    RuleFor(x => x.MicrochippedDate).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage("Grace");
        //}); 
        
        When(x => IsEmptyDate(x), () =>
        {
            RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(x => localizer[MicrochipError]);
        });

        When(x => !IsEmptyDate(x) && (x.Day == null), () =>
        {
            RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(localizer[MicrochipError]);
        });
        
        When(x => !IsEmptyDate(x) && (x.Month == null && x.Day != null), () =>
        {
            RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(localizer[MicrochipError]);
        });

        When(x => !IsEmptyDate(x) && (x.Month == null), () =>
        {
            RuleFor(x => x.Month).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(" ");
        });

        When(x => !IsEmptyDate(x) && (x.Year == null && x.Day != null && x.Month != null), () =>
        {
            RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(localizer[MicrochipError]);
        });

        When(x => !IsEmptyDate(x) && (x.Year == null), () =>
        {
            RuleFor(x => x.Year).Cascade(CascadeMode.Stop)
            .NotEmpty().WithMessage(" ");
        });

        //When(x => !x.MicrochippedDate.HasValue && x.Day != null && x.Month != null && x.Year != null, () =>
        //{
        //    RuleFor(x => x.Day).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(localizer[MicrochipError]);
        //});

        //When(x => !IsEmptyDate(x) && (x.Day != null || x.Month != null || x.Year != null), () =>
        //{
        //    RuleFor(x => x.MicrochippedDate).Cascade(CascadeMode.Stop)
        //    .NotEmpty().WithMessage(localizer[MicrochipError]);
        //});

        When(x => x.MicrochippedDate.HasValue && !x.BirthDate.HasValue, () =>
        {
            RuleFor(x => x.MicrochippedDate).Cascade(CascadeMode.Stop)
            .Must(BeTodayOrPastDate).WithMessage(localizer["Enter a date that is in the past"]);

            var message = localizer["Enter a date that is less than 34 years ago"].Value;
            RuleFor(x => x.MicrochippedDate).Must((x, e) => MeetsDateLimits(x.MicrochippedDate, out message)).WithMessage(x => localizer[message]);
            
        });

        When(x => x.BirthDate.HasValue && x.MicrochippedDate.HasValue, () =>
        {
            var message = localizer["Enter a date that is less than 34 years ago"].Value;
            RuleFor(x => x.MicrochippedDate).Cascade(CascadeMode.Stop)
            .Must(BeTodayOrPastDate).WithMessage(localizer["Enter a date that is in the past"])
            .Must((x, e) => MeetsDateLimits(x.MicrochippedDate, out message)).WithMessage(x => localizer[message])
            .GreaterThan(m => m.BirthDate).WithMessage(localizer["Enter a date that is after the pet’s date of birth"]);
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
