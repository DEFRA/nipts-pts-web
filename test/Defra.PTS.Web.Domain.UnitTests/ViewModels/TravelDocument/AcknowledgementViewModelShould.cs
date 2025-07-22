using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Testing.Common.Helpers;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Reflection;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels.TravelDocument;

public class AcknowledgementViewModelShould
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
    [InlineData("Reference")]
    [InlineData("IsSuccess")]
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
        result.Should().Be(Enums.TravelDocumentFormPageType.Acknowledgement);
    }

    [Fact]
    public void HaveCorrectTitle()
    {
        // Act
        var result = AcknowledgementViewModel.FormTitle;

        // Assert
        result.Should().Be("Application submitted");
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
        model.IsSuccess = true;
        model.Reference = "Reference";

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.Acknowledgement);
            model.IsCompleted.Should().Be(true);
            model.IsSuccess.Should().Be(true);
            model.Reference.Should().Be("Reference");
        }
    }

    [Fact]
    public void VerifyClearDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.IsSuccess = true;
        model.Reference = "Reference";

        // Act
        model.ClearData();

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.Acknowledgement);
            model.IsCompleted.Should().Be(default);
            model.IsSuccess.Should().Be(default);
            model.Reference.Should().Be(default);
        }
    }

    [Fact]
    public void VerifyTrimUnwantedDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.IsSuccess = true;
        model.Reference = "Reference";

        // Act
        model.TrimUnwantedData();

        // Assert
        using (new AssertionScope())
        {
            Assert.True(model.IsCompleted);
            Assert.True(model.IsSuccess);
            Assert.Equal("Reference", model.Reference);
        }
    }

    #region PrivateMethods
    private static AcknowledgementViewModel CreateModel() => new();
    private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<AcknowledgementViewModel>(propertyName);
    #endregion PrivateMethods
}
