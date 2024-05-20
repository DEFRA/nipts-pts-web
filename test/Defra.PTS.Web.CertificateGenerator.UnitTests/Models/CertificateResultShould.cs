using System.Text;

namespace Defra.PTS.Web.CertificateGenerator.UnitTests.Models;

public class CertificateResultShould
{
    private const string MockName = "TestName";
    private const string MockMimeType = "TestMimeType";

    [Fact]
    public void ShouldContainTheExpectedConstructor()
    {
        var sut = new CertificateResult(MockName, Stream.Null, MockMimeType);

        _ = sut.Should().NotBeNull();
    }

    [Fact]
    public void ShouldSetTheExpectedName()
    {
        var sut = new CertificateResult(MockName, Stream.Null, MockMimeType);

        _ = sut.Name.Should().Be(MockName);
    }

    [Fact]
    public void ShouldSetTheExpectedMimeType()
    {
        var sut = new CertificateResult(MockName, Stream.Null, MockMimeType);

        _ = sut.MimeType.Should().Be(MockMimeType);
    }

    [Fact]
    public void ShouldSetTheExpectedContentStream()
    {
        using var testStream = new MemoryStream(Encoding.UTF8.GetBytes("Test Text"));
        var sut = new CertificateResult(MockName, testStream, MockMimeType);

        _ = sut.Content.Should().BeReadable();
    }
}