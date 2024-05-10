using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;

namespace Defra.PTS.Web.Application.UnitTests.Validation;

public class PetGenderValidatorShould
{
    public PetGenderValidatorShould()
    {
    }

    [Fact]
    public async Task HaveErrorWhenGenderIsNotSpecified()
    {
        // Arrange
        var model = new PetGenderViewModel { };
        var validator = CreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.Gender);
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

    private static PetGenderValidator CreateValidator() => new();
}
