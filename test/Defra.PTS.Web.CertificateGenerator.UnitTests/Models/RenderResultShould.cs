namespace Defra.PTS.Web.CertificateGenerator.UnitTests.Models;

public class RenderResultShould
{
    private const string MockContent = "TestContent";
    private const string MockName = "TestName";

    [Fact]
    public void ShouldSetTheExpectedName()
    {
        var sut = new RenderResult<string>(MockContent, MockName);

        _ = sut.Name.Should().Be(MockName);
    }

    [Fact]
    public void ShouldSetTheExpectedContentString()
    {
        var sut = new RenderResult<string>(MockContent, MockName);

        _ = sut.Content.Should().Be(MockContent);
    }
}