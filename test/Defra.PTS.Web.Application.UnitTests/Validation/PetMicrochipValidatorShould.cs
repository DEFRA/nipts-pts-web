using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetMicrochipValidatorShould
    {
        [Theory]
        [MemberData(nameof(PetMicrochipValidatorTestData))]
        public async Task HaveErrorIfMicroChipNumberInvalid(string microchipNumber, string expectedErrorMessage)
        {
            // Arrange
            var model = new PetMicrochipViewModel { Microchipped = YesNoOptions.Yes, MicrochipNumber = microchipNumber };
            var validator = new PetMicrochipValidator();

            // Act
            var result = await validator.TestValidateAsync(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.MicrochipNumber)
                  .WithErrorMessage(expectedErrorMessage);
        }

        public static IEnumerable<object[]> PetMicrochipValidatorTestData()
        {
            yield return new object[] { string.Empty, "Enter your pet's microchip number" };
            yield return new object[] { new string('1', 14), "Enter your pet’s 15-digit microchip number" };
            yield return new object[] { new string('1', 16), "Enter your pet’s 15-digit microchip number" };
            yield return new object[] { new string('a', 15), "Enter a 15-digit number, using only numbers" };
        }
    }
}
