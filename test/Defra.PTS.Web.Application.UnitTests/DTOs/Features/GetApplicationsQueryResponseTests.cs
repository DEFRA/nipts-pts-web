using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Testing.Common.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Defra.PTS.Web.Application.UnitTests.DTOs.Features;

public class GetApplicationsQueryResponseTests
{
 
    [Theory]
    [InlineData("UserId")]
    [InlineData("Applications")]
    public void GetApplicationsQueryResponse_HavePropertiesWithGettersAndSetters(string propertyName)
    {
        // Arrange
        var property = TestHelper.GetPropertyInfo<GetApplicationsQueryResponse>(propertyName);

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
    public void GetApplicationsQueryResponse_HasCorrectData()
    {
        // Arrange
        var model = new GetApplicationsQueryResponse
        {
            UserId = Guid.Empty,
            Applications = new List<Application.DTOs.Services.ApplicationSummaryDto>()
        };

        // Act

        // Assert
        using (new AssertionScope())
        {
            model.UserId.Should().Be(Guid.Empty);
            model.Applications.Should().NotBeNull();
        }
    }
}
