using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Defra.PTS.Web.Domain;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Defra.PTS.Web.Application.UnitTests.Validation;

public class PetNameValidatorShould
{
    private readonly IStringLocalizer<ISharedResource> _localizer;
    public PetNameValidatorShould()
    {
        var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
        var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
        _localizer = new StringLocalizer<ISharedResource>(factory);
    }

    [Fact]
    public async Task HaveErrorWhenNameIsNotSpecified()
    {
        // Arrange
        var model = new PetNameViewModel { PetName = null };
        var validator = new PetNameValidator(_localizer);

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
        var validator = new PetNameValidator(_localizer);

        // Act
        var result = await validator.TestValidateAsync(model);

        // Assert
        result.ShouldNotHaveValidationErrorFor(x => x.PetName);
    }

}
