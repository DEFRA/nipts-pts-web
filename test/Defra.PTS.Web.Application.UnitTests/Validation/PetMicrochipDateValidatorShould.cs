using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Defra.PTS.Web.Application.UnitTests.Validation;

public class PetMicrochipDateValidatorShould
{
    private readonly IStringLocalizer<ISharedResource> _localizer;
    public PetMicrochipDateValidatorShould()
    {
        var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
        var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
        _localizer = new StringLocalizer<ISharedResource>(factory);
    }
    [Fact]
    public async Task NotHaveErrorMicrochippedDate()
    {
        var model = new PetMicrochipDateViewModel()
        {
            Year = "2024",
            Month = "1",
            Day = "1",
        };
        var validator = new PetMicrochipDateValidator(_localizer);

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(x => x.Year);
        result.ShouldNotHaveValidationErrorFor(x => x.Month);
        result.ShouldNotHaveValidationErrorFor(x => x.Day);
    }

    [Fact]
    public async Task HaveErrorIfMicrochippedDateEmpty()
    {
        var model = new PetMicrochipDateViewModel();
        var validator = new PetMicrochipDateValidator(_localizer);

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.Day);
        result.ShouldHaveValidationErrorFor(x => x.Month);
        result.ShouldHaveValidationErrorFor(x => x.Year);
    }

    [Fact]
    public async Task HaveErrorIfDayEmpty()
    {
        var model = new PetMicrochipDateViewModel() { Month = "1", Year = "2010" };
        var validator = new PetMicrochipDateValidator(_localizer);

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.Day);
    }

    [Fact]
    public async Task HaveErrorIfMonthEmpty()
    {
        var model = new PetMicrochipDateViewModel() { Day = "1", Year = "2010" };
        var validator = new PetMicrochipDateValidator(_localizer);

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.Month);
    }

    [Fact]
    public async Task HaveErrorIfDayMonthEmpty()
    {
        var model = new PetMicrochipDateViewModel() { Year = "2010" };
        var validator = new PetMicrochipDateValidator(_localizer);

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.Day);
        result.ShouldHaveValidationErrorFor(x => x.Month);
    }

    [Fact]
    public async Task HaveErrorIfYearEmpty()
    {
        var model = new PetMicrochipDateViewModel() { Day = "1", Month = "1" };
        var validator = new PetMicrochipDateValidator(_localizer);

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.Year);
    }

    [Fact]
    public async Task HaveErrorWhenMicrochipDateExceedsDateLimits()
    {
        var model = new PetMicrochipDateViewModel()
        {
            Year = "1980",
            Month = "1",
            Day = "1",
            BirthDate = new DateTime(1990, 01, 01)
        };

        var validator = new PetMicrochipDateValidator(_localizer);

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.Day);
        result.ShouldHaveValidationErrorFor(x => x.Month);
        result.ShouldHaveValidationErrorFor(x => x.Year);
    }
}
