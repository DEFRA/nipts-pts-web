using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Testing.Common.Helpers;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Reflection;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels.TravelDocument;

public class DeclarationViewModelShould
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
    [InlineData("AgreedToAccuracy")]
    [InlineData("AgreedToPrivacyPolicy")]
    [InlineData("AgreedToDeclaration")]
    [InlineData("RequestId")]
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
        result.Should().Be(Enums.TravelDocumentFormPageType.Declaration);
    }

    [Fact]
    public void HaveCorrectTitle()
    {
        // Act
        var result = DeclarationViewModel.FormTitle;

        // Assert
        result.Should().Be("Check your answers and sign the declaration");
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
        model.AgreedToAccuracy = true;
        model.AgreedToPrivacyPolicy = true;
        model.AgreedToDeclaration = true;
        model.RequestId = Guid.Empty;

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.Declaration);
            model.IsCompleted.Should().Be(true);
            model.AgreedToAccuracy.Should().Be(true);
            model.AgreedToPrivacyPolicy.Should().Be(true);
            model.AgreedToDeclaration.Should().Be(true);
            model.RequestId.Should().Be(Guid.Empty);
        }
    }

    [Fact]
    public void VerifyClearDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.AgreedToAccuracy = true;
        model.AgreedToPrivacyPolicy = true;
        model.AgreedToDeclaration = true;
        model.RequestId = Guid.Empty;

        // Act
        model.ClearData();

        // Assert
        using (new AssertionScope())
        {
            model.PageType.Should().Be(Enums.TravelDocumentFormPageType.Declaration);
            model.IsCompleted.Should().Be(default);
            model.AgreedToAccuracy.Should().Be(default);
            model.AgreedToPrivacyPolicy.Should().Be(default);
            model.AgreedToDeclaration.Should().Be(default);
        }
    }

    [Fact]
    public void VerifyTrimUnwantedDataWorksAsExpected()
    {
        // Arrange
        var model = CreateModel();

        model.IsCompleted = true;
        model.AgreedToAccuracy = true;
        model.AgreedToPrivacyPolicy = true;
        model.AgreedToDeclaration = true;

        // Act
        model.TrimUnwantedData();

        // Assert
        using (new AssertionScope())
        {
            Assert.True(model.IsCompleted);
            Assert.True(model.AgreedToAccuracy);
            Assert.True(model.AgreedToPrivacyPolicy);
            Assert.True(model.AgreedToDeclaration);
        }
    }

    #region PrivateMethods
    private static DeclarationViewModel CreateModel() => new();
    private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<DeclarationViewModel>(propertyName);
    #endregion PrivateMethods
}
