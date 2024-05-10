using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Testing.Common.Helpers;
using Defra.PTS.Web.Domain.ViewModels.Components;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Reflection;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels.Components;

public class MicrochipInformationCvmShould
{
    [Theory]
    [InlineData("MicrochipDate")]
    [InlineData("MicrochipNumber")]
    [InlineData("MicrochipImplantLocation")]
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
        model.MicrochipDate = "14/03/2023";
        model.MicrochipNumber = "123456789012345";
        model.MicrochipImplantLocation = "Below the shoulder blade";

        // Assert
        using (new AssertionScope())
        {
            model.MicrochipDate.Should().Be("14/03/2023");
            model.MicrochipNumber.Should().Be("123456789012345");
            model.MicrochipImplantLocation.Should().Be("Below the shoulder blade");
        }
    }

    #region PrivateMethods
    private static MicrochipInformationCvm CreateModel() => new();
    private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<MicrochipInformationCvm>(propertyName);
    #endregion PrivateMethods
}
