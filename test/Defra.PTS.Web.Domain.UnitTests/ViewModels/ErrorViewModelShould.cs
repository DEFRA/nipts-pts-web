using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Testing.Common.Helpers;
using Defra.PTS.Web.Domain.ViewModels.Components;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Reflection;
using Defra.PTS.Web.Domain.ViewModels;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels;

public class ErrorViewModelShould
{
    [Theory]
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
    public void HaveValidData()
    {
        // Arrange
        var model = CreateModel();

        // Act
        model.RequestId = "test15-A";

        // Assert
        using (new AssertionScope())
        {
            model.RequestId = "test15-A";
            model.ShowRequestId.Should().Be(true);
        }
    }

    [Fact]
    public void HaveEmptData_ReturnsRequestIdFalse()
    {
        // Arrange
        var model = CreateModel();

        // Act
        model.RequestId = "";

        // Assert
        using (new AssertionScope())
        {
            model.RequestId = "";
            model.ShowRequestId.Should().Be(false);
        }
    }

    #region PrivateMethods
    private static ErrorViewModel CreateModel() => new();
    private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<ErrorViewModel>(propertyName);
    #endregion PrivateMethods
}
