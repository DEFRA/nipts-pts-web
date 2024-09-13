using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Validation;

public class PetAgeValidatorShould
{
    private readonly IStringLocalizer<SharedResource> _localizer;

    public PetAgeValidatorShould()
    {
        var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
        var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
        _localizer = new StringLocalizer<SharedResource>(factory);
    }

    [Fact]
    public async Task HaveErrorWhenBirthDateEmpty()
    {
        var model = new PetAgeViewModel();
        var validator = new PetAgeValidator(_localizer);
        
        var result = await validator.TestValidateAsync(model);
        
        result.ShouldHaveValidationErrorFor(x => x.BirthDate);
    }

    [Fact]
    public async Task NotHaveErrorWhenBirthDateHasValue()
    {
        var model = new PetAgeViewModel()
        {
            Day = 1,
            Month = 1,
            Year = 2005,
            MicrochippedDate = DateTime.Now,
        };
        var validator = new PetAgeValidator(_localizer);

        var result = await validator.TestValidateAsync(model);

        result.ShouldNotHaveValidationErrorFor(x => x.BirthDate);
        result.ShouldNotHaveValidationErrorFor(x => x.Day);
        result.ShouldNotHaveValidationErrorFor(x => x.Month);
        result.ShouldNotHaveValidationErrorFor(x => x.Year);
    }

    [Fact]
    public async Task HaveErrorWhenBirthDateExceedsDateLimits()
    {
        var model = new PetAgeViewModel()
        {
            Day = 1,
            Month = 1,
            Year = DateTime.Now.AddYears(-50).Year,
            MicrochippedDate = DateTime.Now,
        };

        var validator = new PetAgeValidator(_localizer);

        var result = await validator.TestValidateAsync(model);

        result.ShouldHaveValidationErrorFor(x => x.BirthDate);
    }



}
