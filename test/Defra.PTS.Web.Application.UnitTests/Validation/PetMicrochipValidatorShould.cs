using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetMicrochipValidatorShould
    {
        [Fact]
        public async Task NotErrorPetMicrochip()
        {
            var model = new PetMicrochipViewModel()
            {
                Microchipped = Domain.Enums.YesNoOptions.Yes,
                MicrochipNumber = new string('1', 15)
            };

            var validator = new PetMicrochipValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.Microchipped);
            result.ShouldNotHaveValidationErrorFor(x => x.MicrochipNumber);
        }

        [Fact]
        public async Task HaveErrorWhenMicrochippedEmpty()
        {            
            var model = new PetMicrochipViewModel();

            var validator = new PetMicrochipValidator();
            
            var result = await validator.TestValidateAsync(model);            
            
            result.ShouldHaveValidationErrorFor(x => x.Microchipped);
        }

        [Theory]
        [MemberData(nameof(PetMicrochipValidatorNumberTestData))]
        public async Task HaveErrorIfMicroChipNumberInvalid(string microchipNumber)
        {
            var model = new PetMicrochipViewModel() { Microchipped = Domain.Enums.YesNoOptions.Yes, MicrochipNumber = microchipNumber };
            var validator = new PetMicrochipValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.MicrochipNumber);
        }

        public static IEnumerable<object[]> PetMicrochipValidatorNumberTestData => new List<object[]>
        {
            new object[] { string.Empty },
            new object[] { new string('1', 14) },
            new object[] { new string('1', 16) },
            new object[] { new string('a', 15) },
        };
    }
}
