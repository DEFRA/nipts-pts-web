using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Testing.Common.Helpers;
using Defra.PTS.Web.Domain.ViewModels.Components;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Reflection;
using Defra.PTS.Web.Domain.ViewModels;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels;

public class FieldsetLegendViewModelShould
{
    [Theory]
    [InlineData("Label")]
    [InlineData("LabelFor")]
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
        model.Label = "test label";
        model.LabelFor = "test label pointer";

        // Assert
        using (new AssertionScope())
        {
            model.Label.Should().Be("test label");
            model.LabelFor.Should().Be("test label pointer");
        }
    }

    #region PrivateMethods
    private static FieldsetLegendViewModel CreateModel() => new();
    private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<FieldsetLegendViewModel>(propertyName);
    #endregion PrivateMethods
}
