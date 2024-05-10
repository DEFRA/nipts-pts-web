using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;

namespace Defra.PTS.Web.Application.UnitTests.Validation;

public class PetNameValidatorShould
{
    public PetNameValidatorShould()
    {
    }

    [Fact]
    public async Task HaveErrorWhenNameIsNotSpecified()
    {
        // Arrange
        var model = new PetNameViewModel { PetName = null };
        var validator = CreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PetName);
    }

    [Fact]
    public async Task NotHaveErrorWhenNameIsSpecified()
    {
        // Arrange
        var model = new PetNameViewModel { PetName = "Teddy" };
        var validator = CreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PetName);
    }

    private static PetNameValidator CreateValidator() => new();
}
