using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Testing.Common.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Defra.PTS.Web.Application.UnitTests.DTOs.Features;

public class GetApplicationDetailsQueryResponseTests
{
    [Theory]
    [InlineData("ApplicationId")]
    [InlineData("ApplicationDetails")]
    public void GetApplicationDetailsQueryResponse_HavePropertiesWithGettersAndSetters(string propertyName)
    {
        // Arrange
        var property = TestHelper.GetPropertyInfo<GetApplicationDetailsQueryResponse>(propertyName);

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
    public void GetApplicationDetailsQueryResponse_HasCorrectData()
    {
        // Arrange
        var model = new GetApplicationDetailsQueryResponse
        {
            ApplicationId = Guid.Empty,
            ApplicationDetails = new Application.DTOs.Services.ApplicationDetailsDto()
        };

        // Act

        // Assert
        using (new AssertionScope())
        {
            model.ApplicationId.Should().Be(Guid.Empty);
            model.ApplicationDetails.Should().NotBeNull();
        }
    }
}
