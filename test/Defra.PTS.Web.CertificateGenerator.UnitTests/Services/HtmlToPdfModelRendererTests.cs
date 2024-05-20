namespace Defra.PTS.Web.CertificateGenerator.UnitTests.Services;

public class HtmlToPdfModelRendererTests
{
    private readonly Mock<IHtmlToPdfConverter> converter;
    private readonly Mock<IHtmlModelRenderer> htmlRenderer;
    private readonly HtmlToPdfModelRenderer<TestContentModel> renderer;

    public record TestContentModel();

    public HtmlToPdfModelRendererTests()
    {
        htmlRenderer = new Mock<IHtmlModelRenderer>(MockBehavior.Strict);
        converter = new Mock<IHtmlToPdfConverter>(MockBehavior.Strict);
        renderer = new HtmlToPdfModelRenderer<TestContentModel>(htmlRenderer.Object, converter.Object);
    }

    [Fact]
    public async Task RenderAsync_RendersCorrectly()
    {
        // arrange
        var model = new TestContentModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var fileId = Guid.NewGuid();
        var contentResult = new RenderResult<string>(Guid.NewGuid().ToString(), $"{fileId}.html");
        var converterResult = Mock.Of<Stream>(MockBehavior.Strict);

        htmlRenderer.Setup(m => m.RenderAsync(model, cancellationToken)).ReturnsAsync(contentResult).Verifiable();
        converter.Setup(m => m.ConvertAsync(It.Is<HtmlToPdfContext>(ctx =>
            ctx.Content == contentResult.Content &&
            ctx.FooterTemplate == null &&
            ctx.HeaderTemplate == null &&
            ctx.Margin == default), cancellationToken)
        ).ReturnsAsync(converterResult).Verifiable();

        // act
        var actual = await renderer.RenderAsync(model, cancellationToken);

        // assert
        _ = actual.Should().NotBeNull();
        _ = actual.Content.Should().BeSameAs(converterResult);
        _ = actual.Name.Should().Be($"{fileId}.pdf");

        htmlRenderer.Verify();
        converter.Verify();
    }

    [Fact]
    public async Task RenderAsync_RendersCorrectly_WhenTheRendererDoesntReturnAHtmlExtension()
    {
        // arrange
        var model = new TestContentModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var fileId = Guid.NewGuid();
        var contentResult = new RenderResult<string>(Guid.NewGuid().ToString(), $"{fileId}.aaaa");
        var converterResult = Mock.Of<Stream>(MockBehavior.Strict);

        htmlRenderer.Setup(m => m.RenderAsync(model, cancellationToken)).ReturnsAsync(contentResult).Verifiable();
        converter.Setup(m => m.ConvertAsync(It.Is<HtmlToPdfContext>(ctx =>
            ctx.Content == contentResult.Content &&
            ctx.FooterTemplate == null &&
            ctx.HeaderTemplate == null &&
            ctx.Margin == default), cancellationToken)
        ).ReturnsAsync(converterResult).Verifiable();

        // act
        var actual = await renderer.RenderAsync(model, cancellationToken);

        // assert
        _ = actual.Should().NotBeNull();
        _ = actual.Content.Should().BeSameAs(converterResult);
        _ = actual.Name.Should().Be($"{fileId}.aaaa.pdf");

        htmlRenderer.Verify();
        converter.Verify();
    }
}