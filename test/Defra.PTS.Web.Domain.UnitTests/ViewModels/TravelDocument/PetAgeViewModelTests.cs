using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Testing.Common.Helpers;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Reflection;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels.TravelDocument;

public class PetAgeViewModelTests
{
    [Theory]
    [InlineData("FormTitle")]
    [InlineData("PageType")]
    [InlineData("BirthDate")]
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
    [InlineData("Day")]
    [InlineData("Month")]
    [InlineData("Year")]
    [InlineData("MicrochippedDate")]
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
        result.Should().Be(Enums.TravelDocumentFormPageType.PetAge);
    }

    [Fact]
    public void HaveCorrectTitle()
    {
        // Act
        var result = PetAgeViewModel.FormTitle;

        // Assert
        result.Should().Be("What is your pet's date of birth?");
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
        model.Day = "1";
        model.Month = "1";
        model.Year = "2020";
        model.MicrochippedDate = new DateTime(2020, 1, 1);

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetAge);
            model.IsCompleted.Should().Be(true);
            model.Day.Should().Be("1");
            model.Month.Should().Be("1");
            model.Year.Should().Be("2020");
            model.MicrochippedDate.Should().Be(new DateTime(2020, 1, 1));
            model.BirthDate.Should().Be(new DateTime(2020, 1, 1));
        }
    }

    [Fact]
    public void VerifyClearDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.Day = 1.ToString();
        model.Month = 1.ToString();
        model.Year = 2020.ToString();
        model.MicrochippedDate = new DateTime(2020, 1, 1);

        // Act
        model.ClearData();

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetAge);
            model.IsCompleted.Should().Be(default);
            model.Day.Should().Be(null);
            model.Month.Should().Be(null);
            model.Year.Should().Be(null);
            model.MicrochippedDate.Should().Be(default);
        }
    }

    [Fact]
    public void VerifyTrimUnwantedDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.Day = 1.ToString();
        model.Month = 1.ToString();
        model.Year = 2020.ToString();
        model.MicrochippedDate = new DateTime(2020, 1, 1);

        // Act
        model.TrimUnwantedData();

        // Assert
        using (new AssertionScope())
        {
            Assert.Equal(1.ToString(), model.Day);
            Assert.Equal(1.ToString(), model.Month);
            Assert.Equal(2020.ToString(), model.Year);
            Assert.Equal(expected: new DateTime(2020, 1, 1), model.MicrochippedDate);
        }
    }

    [Fact]
    public void VerifyNullDateWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.Day = 32.ToString();
        model.Month = 13.ToString();
        model.Year = 2020.ToString();
        model.MicrochippedDate = new DateTime(2020, 1, 1);

        // Act
        model.TrimUnwantedData();

        // Assert
        using (new AssertionScope())
        {
            model.BirthDate.Should().Be(null);
        }
    }

    #region PrivateMethods
    private static PetAgeViewModel CreateModel() => new();
    private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<PetAgeViewModel>(propertyName);
    #endregion PrivateMethods
}
