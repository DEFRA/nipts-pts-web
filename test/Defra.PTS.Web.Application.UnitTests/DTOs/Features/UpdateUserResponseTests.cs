using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Testing.Common.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Defra.PTS.Web.Application.UnitTests.DTOs.Features;

public class UpdateUserResponseTests
{
    [Theory]
    [InlineData("IsSuccess")]
    [InlineData("UserId")]
    public void UpdateUserResponse_HavePropertiesWithGettersAndSetters(string propertyName)
    {
        // Arrange
        var property = TestHelper.GetPropertyInfo<UpdateUserResponse>(propertyName);

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
    public void UpdateUserResponse_HasCorrectData()
    {
        // Arrange
        var model = new UpdateUserResponse
        {
            IsSuccess = true,
            UserId = Guid.Empty
        };

        // Act

        // Assert
        using (new AssertionScope())
        {
            model.IsSuccess.Should().Be(true);
            model.UserId.Should().Be(Guid.Empty);
        }
    }

}
