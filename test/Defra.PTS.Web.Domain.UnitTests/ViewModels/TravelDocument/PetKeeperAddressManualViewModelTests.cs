using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Testing.Common.Helpers;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Reflection;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels.TravelDocument;

public class PetKeeperAddressManualViewModelTests
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
    [InlineData("AddressLineOne")]
    [InlineData("AddressLineTwo")]
    [InlineData("TownOrCity")]
    [InlineData("County")]
    [InlineData("Postcode")]
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
        result.Should().Be(Enums.TravelDocumentFormPageType.PetKeeperAddressManual);
    }

    [Fact]
    public void HaveCorrectTitle()
    {
        // Arrange
        var model = CreateModel();

        // Act
        var result = model.FormTitle;

        // Assert
        result.Should().Be($"What is your address?");
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
        model.AddressLineOne = "AddressLineOne";
        model.AddressLineTwo = "AddressLineTwo";
        model.TownOrCity = "TownOrCity";
        model.County = "County";
        model.Postcode = "Postcode";

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetKeeperAddressManual);
            model.IsCompleted.Should().Be(true);
            model.AddressLineOne.Should().Be("AddressLineOne");
            model.AddressLineTwo.Should().Be("AddressLineTwo");
            model.TownOrCity.Should().Be("TownOrCity");
            model.County.Should().Be("County");
            model.Postcode.Should().Be("Postcode");
        }
    }

    [Fact]
    public void VerifyClearDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.AddressLineOne = "AddressLineOne";
        model.AddressLineTwo = "AddressLineTwo";
        model.TownOrCity = "TownOrCity";
        model.County = "County";
        model.Postcode = "Postcode";

        // Act
        model.ClearData();

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetKeeperAddressManual);
            model.IsCompleted.Should().Be(default);
            model.AddressLineOne.Should().Be(default);
            model.AddressLineTwo.Should().Be(default);
            model.TownOrCity.Should().Be(default);
            model.County.Should().Be(default);
            model.Postcode.Should().Be(default);
        }
    }

    [Fact]
    public void VerifyTrimUnwantedDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.AddressLineOne = "AddressLineOne";
        model.AddressLineTwo = "AddressLineTwo";
        model.TownOrCity = "TownOrCity";
        model.County = "County";
        model.Postcode = "Postcode";
        model.PostcodeRegion = PostcodeRegion.GB;

        // Act
        model.TrimUnwantedData();

        // Assert
        using (new AssertionScope())
        {
            Assert.True(model.IsCompleted);
            Assert.Equal("AddressLineOne", model.AddressLineOne);
            Assert.Equal("AddressLineTwo", model.AddressLineTwo);
            Assert.Equal("TownOrCity", model.TownOrCity);
            Assert.Equal("County", model.County);
            Assert.Equal("Postcode", model.Postcode);
            Assert.Equal(PostcodeRegion.GB, model.PostcodeRegion);
        }
    }

    [Theory]
    [InlineData("FakeData")]
    public void VerifyExceptionThrownAsExpected(string propertyName)
    {
        // Arrange
        var model = CreateModel();

        // Act
        model.TrimUnwantedData();

        // Assert
        Assert.Throws<ArgumentException>(() => GetPropertyInfo(propertyName));
    }

    #region PrivateMethods
    private static PetKeeperAddressManualViewModel CreateModel() => new();
    private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<PetKeeperAddressManualViewModel>(propertyName);
    #endregion PrivateMethods
}
