using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using MediatR;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetMicrochipDateValidatorShould
    {
        [Fact]
        public async Task NotHaveErrorMicrochippedDate()
        {
            var model = new PetMicrochipDateViewModel()
            {
                Year = "2024",
                Month = "1",
                Day = "1",
            };
            var validator = new PetMicrochipDateValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.MicrochippedDate);
            result.ShouldNotHaveValidationErrorFor(x => x.Year);
            result.ShouldNotHaveValidationErrorFor(x => x.Month);
            result.ShouldNotHaveValidationErrorFor(x => x.Day);
        }

        [Fact]
        public async Task HaveErrorIfMicrochippedDateEmpty()
        {
            var model = new PetMicrochipDateViewModel();
            var validator = new PetMicrochipDateValidator();

            var result = await validator.TestValidateAsync(model);            

            result.ShouldHaveValidationErrorFor(x => x.MicrochippedDate);
        }

        [Fact]
        public async Task HaveErrorIfDayMonthEmpty()
        {
            var model = new PetMicrochipDateViewModel() { Year = "2010" };
            var validator = new PetMicrochipDateValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.MicrochippedDate);
        }

        [Fact]
        public async Task HaveErrorIfYearEmpty()
        {
            var model = new PetMicrochipDateViewModel() { Day = "1", Month = "1"};
            var validator = new PetMicrochipDateValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.MicrochippedDate);
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

            var validator = new PetMicrochipDateValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.MicrochippedDate);
        }

    }
}
