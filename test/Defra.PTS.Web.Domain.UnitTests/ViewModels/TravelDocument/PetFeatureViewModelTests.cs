using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Testing.Common.Helpers;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Reflection;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels.TravelDocument;

public class PetFeatureViewModelTests
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
    [InlineData("HasUniqueFeature")]
    [InlineData("FeatureDescription")]
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
        result.Should().Be(Enums.TravelDocumentFormPageType.PetFeature);
    }

    [Fact]
    public void HaveCorrectTitle()
    {
        // Act
        var result = PetFeatureViewModel.FormTitle;

        // Assert
        result.Should().Be($"Does your pet have any significant features?");
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
        model.HasUniqueFeature = YesNoOptions.No;
        model.FeatureDescription = "FeatureDescription";

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetFeature);
            model.IsCompleted.Should().Be(true);
            model.HasUniqueFeature.Should().Be(YesNoOptions.No);
            model.FeatureDescription.Should().Be("FeatureDescription");
        }
    }

    [Fact]
    public void VerifyClearDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.HasUniqueFeature = YesNoOptions.No;
        model.FeatureDescription = "FeatureDescription";

        // Act
        model.ClearData();

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.PetFeature);
            model.IsCompleted.Should().Be(default);
            model.HasUniqueFeature.Should().Be(default);
            model.FeatureDescription.Should().Be(default);
        }
    }

    [Fact]
    public void VerifyTrimUnwantedDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.HasUniqueFeature = YesNoOptions.No;
        model.FeatureDescription = "FeatureDescription";

        // Act
        model.TrimUnwantedData();

        // Assert
        using (new AssertionScope())
        {
            Assert.True(model.IsCompleted);
            Assert.Equal(YesNoOptions.No, model.HasUniqueFeature);
            Assert.Equal(string.Empty, model.FeatureDescription);
        }
    }

    [Fact]
    public void VerifyDefaultCaseForYesNoCase()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.FeatureDescription = "FeatureDescription";

        // Act
        model.TrimUnwantedData();

        // Assert
        using (new AssertionScope())
        {
            model.IsCompleted.Should().Be(true);
            model.FeatureDescription.Should().Be("FeatureDescription");
        }
    }

    #region PrivateMethods
    private static PetFeatureViewModel CreateModel() => new();
    private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<PetFeatureViewModel>(propertyName);
    #endregion PrivateMethods
}
