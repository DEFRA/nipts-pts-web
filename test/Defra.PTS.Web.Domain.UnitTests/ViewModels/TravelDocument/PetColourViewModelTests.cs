using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Testing.Common.Helpers;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Reflection;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels.TravelDocument;

public class PetColourViewModelTests
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
    [InlineData("PetColour")]
    [InlineData("PetColourOther")]
    [InlineData("OtherColourID")]
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
        result.Should().Be(Enums.TravelDocumentFormPageType.PetColour);
    }

    [Fact]
    public void HaveCorrectTitle()
    {
        // Arrange
        var model = CreateModel();

        // Act
        var result = model.FormTitle;

        // Assert
        result.Should().Be($"What is the main colour of your {model.PetTypeNameLowered}?");
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
        model.PetColour = 123;
        model.PetColourOther = "PetColourOther";
        model.OtherColourID = 123;

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetColour);
            model.IsCompleted.Should().Be(true);
            model.PetSpecies.Should().Be(PetSpecies.Dog);
            model.PetColour.Should().Be(123);
            model.PetColourOther.Should().Be("PetColourOther");
            model.OtherColourID.Should().Be(123);
        }
    }

    [Fact]
    public void VerifyClearDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.PetSpecies = PetSpecies.Dog;
        model.PetColour = 123;
        model.PetColourOther = "PetColourOther";
        model.OtherColourID = 123;


        // Act
        model.ClearData();

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetColour);
            model.IsCompleted.Should().Be(default);
            model.PetSpecies.Should().Be(default);
            model.PetColour.Should().Be(default);
            model.PetColourOther.Should().Be(default);
        }
    }

    [Fact]
    public void VerifyTrimUnwantedDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.PetSpecies = PetSpecies.Dog;
        model.PetColour = 123;
        model.PetColourOther = "PetColourOther";
        model.OtherColourID = 123456;


        // Act
        model.TrimUnwantedData();

        // Assert
        using (new AssertionScope())
        {
            Assert.True(model.IsCompleted);
            Assert.Equal(PetSpecies.Dog, model.PetSpecies);
            Assert.Equal(123, model.PetColour);
            Assert.Equal(string.Empty, model.PetColourOther);
            Assert.Equal(123456, model.OtherColourID);
        }
    }

    #region PrivateMethods
    private static PetColourViewModel CreateModel() => new();
    private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<PetColourViewModel>(propertyName);
    #endregion PrivateMethods
}
