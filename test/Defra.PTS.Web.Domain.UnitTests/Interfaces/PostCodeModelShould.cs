using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Testing.Common.Helpers;
using Defra.PTS.Web.Domain.ViewModels.Components;
using FluentAssertions;
using FluentAssertions.Execution;
using System.Reflection;
using Defra.PTS.Web.Domain.ViewModels;
using Defra.PTS.Web.Domain.Interfaces;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;

namespace Defra.PTS.Web.Domain.UnitTests.ViewModels;

public class PostcodeModelShould
{
    [Theory]
    [InlineData("Postcode")]
    [InlineData("PostcodeRegion")]
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

    #region PrivateMethods
    private static PropertyInfo GetPropertyInfo(string propertyName) => TestHelper.GetPropertyInfo<IPostcodeModel>(propertyName);
    #endregion PrivateMethods
}
