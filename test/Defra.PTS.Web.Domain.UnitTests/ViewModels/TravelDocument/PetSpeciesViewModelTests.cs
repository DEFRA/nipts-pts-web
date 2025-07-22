using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Testing.Common.Helpers;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Reflection;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels.TravelDocument;

public class PetSpeciesViewModelTestss
{
    [Theory]
    [InlineData("FormTitle")]
    [InlineData("PageType")]
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
        result.Should().Be(Enums.TravelDocumentFormPageType.PetSpecies);
    }

    [Fact]
    public void HaveCorrectTitle()
    {
        // Act
        var result = PetSpeciesViewModel.FormTitle;

        // Assert
        result.Should().Be($"Is your pet a dog, cat or ferret?");
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

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetSpecies);
            model.IsCompleted.Should().Be(true);
        }
    }

    [Fact]
    public void VerifyClearDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.PetSpecies = PetSpecies.Dog;

        // Act
        model.ClearData();

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetSpecies);
            model.IsCompleted.Should().Be(default);
            model.PetSpecies.Should().Be(default);
        }
    }

    [Fact]
    public void VerifyTrimUnwantedDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.PetSpecies = PetSpecies.Dog;


        // Act
        model.TrimUnwantedData();

        // Assert
        using (new AssertionScope())
        {
            Assert.True(model.IsCompleted);
            Assert.Equal(PetSpecies.Dog, model.PetSpecies);
        }
    }

    #region PrivateMethods
    private static PetSpeciesViewModel CreateModel() => new();
    private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<PetSpeciesViewModel>(propertyName);
    #endregion PrivateMethods
}
