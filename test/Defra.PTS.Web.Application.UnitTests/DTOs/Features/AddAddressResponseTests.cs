using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Testing.Common.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Defra.PTS.Web.Application.UnitTests.DTOs.Features;

public class AddAddressResponseTests
{
    [Theory]
    [InlineData("IsSuccess")]
    public void AddAddressResponse_HavePropertiesWithGettersAndSetters(string propertyName)
    {
        // Arrange
        var property = TestHelper.GetPropertyInfo<AddAddressResponse>(propertyName);

        // Act

        // Assert
        using (new AssertionScope())
        {
            property.Should().NotBeNull();
            property.Should().BeWritable();
            property.Should().BeReadable();
        }
    }

    [Fact]
    public void AddAddressResponse_HasCorrectData()
    {
        // Arrange
        var model = new AddAddressResponse
        {
            IsSuccess = true
        };

        // Act

        // Assert
        model.IsSuccess.Should().Be(true);
    }
}
