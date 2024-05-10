using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Testing.Common.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Defra.PTS.Web.Application.UnitTests.DTOs.Features;

public class GetApplicationCertificateQueryResponseTests
{
    [Theory]
    [InlineData("ApplicationId")]
    [InlineData("ApplicationCertificate")]
    public void GetApplicationCertificateQueryResponse_HavePropertiesWithGettersAndSetters(string propertyName)
    {
        // Arrange
        var property = TestHelper.GetPropertyInfo<GetApplicationCertificateQueryResponse>(propertyName);

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
    public void GetApplicationCertificateQueryResponse_HasCorrectData()
    {
        // Arrange
        var model = new GetApplicationCertificateQueryResponse
        {
            ApplicationId = Guid.Empty,
            ApplicationCertificate = new Application.DTOs.Services.ApplicationCertificateDto()
        };

        // Act

        // Assert
        using (new AssertionScope())
        {
            model.ApplicationId.Should().Be(Guid.Empty);
            model.ApplicationCertificate.Should().NotBeNull();
        }
    }
}
