using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Testing.Common.Helpers;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Reflection;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels.TravelDocument;

public class PetBreedViewModelTests
{
    [Theory]
    [InlineData("FormTitle")]
    [InlineData("PageType")]
    [InlineData("PetTypeNameLowered")]
    public void HavePropertiesWithOnlyGetters(string propertyName)
    {
        // Arrange
        var property = GetPropertyInfo(propertyName);

        // Act

        // Assert
        using (new AssertionScope())
        {
            property.Should().NotBeWritable();
            property.Should().BeReadable();
        }
    }

    [Theory]
    [InlineData("IsCompleted")]
    [InlineData("PetSpecies")]
    [InlineData("BreedName")]
    [InlineData("BreedId")]
    public void HavePropertiesWithGettersAndSetters(string propertyName)
    {
        // Arrange
        var property = GetPropertyInfo(propertyName);

        // Act

        // Assert
        using (new AssertionScope())
        {
            property.Should().BeWritable();
            property.Should().BeReadable();
        }
    }

    [Fact]
    public void HaveCorrectPageType()
    {
        // Arrange
        var model = CreateModel();

        // Act
        var result = model.PageType;

        // Assert
        result.Should().Be(Enums.TravelDocumentFormPageType.PetBreed);
    }

    [Fact]
    public void HaveCorrectTitle()
    {
        // Arrange
        var model = CreateModel();

        // Act
        var result = model.FormTitle;

        // Assert
        result.Should().Be($"What breed is your {model.PetTypeNameLowered}?");
    }

    [Fact]
    public void BeAssignableToTravelDocumentFormPage()
    {
        // Arrange
        var model = CreateModel();

        // Assert
        model.Should().BeAssignableTo<Domain.ViewModels.TravelDocument.TravelDocumentFormPage>();
    }

    [Fact]
    public void HaveValidData()
    {
        // Arrange
        var model = CreateModel();

        // Act
        model.IsCompleted = true;
        model.PetSpecies = PetSpecies.Dog;
        model.BreedName = "BreedName";
        model.BreedId = 123;
        model.BreedAdditionalInfo = "Mixed with a Corgi";

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetBreed);
            model.IsCompleted.Should().Be(true);
            model.PetSpecies.Should().Be(PetSpecies.Dog);
            model.BreedName.Should().Be("BreedName");
            model.BreedId.Should().Be(123);
            model.BreedAdditionalInfo.Should().Be("Mixed with a Corgi");
        }
    }

    [Fact]
    public void VerifyClearDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.PetSpecies = PetSpecies.Dog;
        model.BreedName = "BreedName";
        model.BreedId = 123;

        // Act
        model.ClearData();

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetBreed);
            model.IsCompleted.Should().Be(default);
            model.PetSpecies.Should().Be(default);
            model.BreedName.Should().Be(default);
            model.BreedId.Should().Be(default);
        }
    }

    [Fact]
    public void VerifyTrimUnwantedDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.PetSpecies = PetSpecies.Dog;
        model.BreedName = "BreedName";
        model.BreedId = 123;

        // Act
        model.TrimUnwantedData();

        // Assert
        using (new AssertionScope())
        {
            Assert.Equal(PetSpecies.Dog, model.PetSpecies);
            Assert.Equal("BreedName", model.BreedName);
            Assert.Equal(123, model.BreedId);
        }
    }

    #region PrivateMethods
    private static PetBreedViewModel CreateModel() => new();
    private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<PetBreedViewModel>(propertyName);
    #endregion PrivateMethods
}
