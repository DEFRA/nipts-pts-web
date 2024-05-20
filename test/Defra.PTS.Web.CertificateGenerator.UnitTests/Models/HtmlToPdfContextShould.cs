namespace Defra.PTS.Web.CertificateGenerator.UnitTests.Models;

public class HtmlToPdfContextShould
{
    private const string MockContent = "TestContent";
    private const string MockFooterTemplate = "TestFooterTemplate";
    private const string MockHeaderTemplate = "TestHeaderTemplate";

    [Fact]
    public void ShouldSetTheExpectedContent()
    {
        var sut = new HtmlToPdfContext { Content = MockContent };

        _ = sut.Content.Should().Be(MockContent);
    }

    [Fact]
    public void ShouldSetTheExpectedFooterTemplate()
    {
        var sut = new HtmlToPdfContext { FooterTemplate = MockFooterTemplate };

        _ = sut.FooterTemplate.Should().Be(MockFooterTemplate);
    }

    [Fact]
    public void ShouldSetTheExpectedHeaderTemplate()
    {
        var sut = new HtmlToPdfContext { HeaderTemplate = MockHeaderTemplate };

        _ = sut.HeaderTemplate.Should().Be(MockHeaderTemplate);
    }

    [Fact]
    public void ShouldSetTheExpectedMargin()
    {
        var mockMargin = new MarginSize(1, 2, 3, 4);

        var sut = new HtmlToPdfContext { Margin = mockMargin };

        _ = sut.Margin.Should().Be(mockMargin);
    }
}