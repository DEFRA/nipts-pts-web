using Moq;
using FluentValidation.TestHelper;
using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Microsoft.Extensions.Localization;
using Defra.PTS.Web.Domain;

namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetGenderValidatorShould
    {
        [Fact]
        public async Task HaveErrorWhenGenderIsNotSpecified()
        {
            // Arrange
            var model = new PetGenderViewModel { }; // Gender is not specified
            var validator = CreateValidator();

            // Act
            var result = await validator.TestValidateAsync(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Gender)
                  .WithErrorMessage("Tell us if your pet is male or female");
        }

        [Theory]
        [InlineData(PetGender.Male)]
        [InlineData(PetGender.Female)]
        public async Task NotHaveErrorWhenGenderIsSpecified(PetGender gender)
        {
            // Arrange
            var model = new PetGenderViewModel { Gender = gender };
            var validator = CreateValidator();

            // Act
            var result = await validator.TestValidateAsync(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Gender);
        }

        private static PetGenderValidator CreateValidator()
        {
            // Mock IStringLocalizer<SharedResource>
            var mockLocalizer = new Mock<IStringLocalizer<SharedResource>>();
            mockLocalizer.Setup(l => l["Tell us if your pet is male or female"]).Returns(new LocalizedString("Tell us if your pet is male or female", "Tell us if your pet is male or female"));

            // Create PetGenderValidator with mocked localizer
            return new PetGenderValidator(mockLocalizer.Object);
        }
    }
}
