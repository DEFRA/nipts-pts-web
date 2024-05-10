using Defra.PTS.Web.UI.Extensions;
using FluentAssertions;
using FluentAssertions.Execution;
using Xunit;

namespace Defra.PTS.Web.UI.UnitTests.Extensions;

public class HttpClientExtensionTests
{
    [Fact]
    public void AddHeaderAccessToken_Returns_CorrectValue()
    {
        // Arrange
        var httpClient = new HttpClient();

        // Act
        httpClient.AddHeaderAccessToken();

        // Assert
        using (new AssertionScope())
        {
            httpClient.DefaultRequestHeaders.Contains("Authorization").Should().Be(true);
        }
    }
}
