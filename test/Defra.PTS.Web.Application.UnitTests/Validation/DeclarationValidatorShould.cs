using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using MediatR;
using Moq;

namespace Defra.PTS.Web.Application.UnitTests.Validation;

public class DeclarationValidatorShould
{
    public DeclarationValidatorShould()
    {
    }

    [Fact]
    public async Task NotHaveErrorWhenAgreedToAccuracy()
    {
        // Arrange
        var model = new DeclarationViewModel { AgreedToAccuracy = true };
        var validator = CreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.AgreedToAccuracy);
    }

    [Fact]
    public async Task HaveErrorWhenNotAgreedToAccuracy()
    {
        // Arrange
        var model = new DeclarationViewModel { AgreedToAccuracy = false };
        var validator = CreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AgreedToAccuracy);
    }

    [Fact]
    public async Task NotHaveErrorWhenAgreedToPrivacyPolicy()
    {
        // Arrange
        var model = new DeclarationViewModel { AgreedToPrivacyPolicy = true };
        var validator = CreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.AgreedToPrivacyPolicy);
    }

    [Fact]
    public async Task HaveErrorWhenNotAgreedToPrivacyPolicy()
    {
        // Arrange
        var model = new DeclarationViewModel { AgreedToPrivacyPolicy = false };
        var validator = CreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AgreedToPrivacyPolicy);
    }

    [Fact]
    public async Task NotHaveErrorWhenAgreedToDeclaration()
    {
        // Arrange
        var model = new DeclarationViewModel { AgreedToDeclaration = true };
        var validator = CreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.AgreedToDeclaration);
    }

    [Fact]
    public async Task HaveErrorWhenNotAgreedToDeclaration()
    {
        // Arrange
        var model = new DeclarationViewModel { AgreedToDeclaration = false };
        var validator = CreateValidator();

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldHaveValidationErrorFor(x => x.AgreedToDeclaration);
    }

    private static DeclarationValidator CreateValidator()
    {
        var mediator = new Mock<IMediator>();  
        return new DeclarationValidator(mediator.Object);
    }
}
