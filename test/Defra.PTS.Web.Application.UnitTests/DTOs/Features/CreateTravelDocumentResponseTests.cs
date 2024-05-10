using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Testing.Common.Helpers;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Defra.PTS.Web.Application.UnitTests.DTOs.Features;

public class CreateTravelDocumentResponseTests
{
    [Theory]
    [InlineData("IsSuccess")]
    [InlineData("Reference")]
    [InlineData("UserId")]
    [InlineData("Application")]
    public void CreateTravelDocumentResponse_HavePropertiesWithGettersAndSetters(string propertyName)
    {
        // Arrange
        var property = TestHelper.GetPropertyInfo<CreateTravelDocumentResponse>(propertyName);

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
    public void CreateTravelDocumentResponse_HasCorrectData()
    {
        // Arrange
        var model = new CreateTravelDocumentResponse
        {
            IsSuccess = true,
            Reference = "ABC123XYZ",
            UserId = Guid.Empty,
            Application = new Application.DTOs.Services.ApplicationDto { Id = Guid.Empty }
        };

        // Act

        // Assert
        using (new AssertionScope())
        {
            model.IsSuccess.Should().Be(true);
            model.UserId.Should().Be(Guid.Empty);
            model.Reference.Should().Be("ABC123XYZ");
        }
    }
}
