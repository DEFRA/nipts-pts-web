using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Testing.Common.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Defra.PTS.Web.Application.UnitTests.DTOs.Features;

public class GetUserDetailQueryResponseTests
{
    [Theory]
    [InlineData("UserId")]
    [InlineData("UserDetail")]
    public void GetUserDetailQueryResponse_HavePropertiesWithGettersAndSetters(string propertyName)
    {
        // Arrange
        var property = TestHelper.GetPropertyInfo<GetUserDetailQueryResponse>(propertyName);

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
    public void GetUserDetailQueryResponse_HasCorrectData()
    {
        // Arrange
        var model = new GetUserDetailQueryResponse
        {
            UserId = Guid.Empty,
            UserDetail = new Application.DTOs.Services.UserDetailDto()
        };

        // Act

        // Assert
        using (new AssertionScope())
        {
            model.UserId.Should().Be(Guid.Empty);
            model.UserDetail.Should().NotBeNull();
        }
    }
}
