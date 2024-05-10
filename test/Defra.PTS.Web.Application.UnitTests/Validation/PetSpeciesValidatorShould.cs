using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;

namespace Defra.PTS.Web.Application.UnitTests.Validation;

public class PetSpeciesValidatorShould
{
    public PetSpeciesValidatorShould()
    {
    }

    [Fact]
    public async Task HaveErrorWhenSpeciesIsNotSpecified()
    {
        // Arrange
        var model = new PetSpeciesViewModel { };
        var validator = CreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.PetSpecies);
    }

    [Theory]
    [InlineData(PetSpecies.Dog)]
    [InlineData(PetSpecies.Cat)]
    [InlineData(PetSpecies.Ferret)]
    public async Task NotHaveErrorWhenSpeciesIsSpecified(PetSpecies species)
    {
        // Arrange
        var model = new PetSpeciesViewModel { PetSpecies = species };
        var validator = CreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PetSpecies);
    }

    private static PetSpeciesValidator CreateValidator() => new();
}
