using Microsoft.Extensions.Options;

namespace Defra.PTS.Web.CertificateGenerator.UnitTests.Services;

public class ConfigurableHtmlToPdfModelRendererTests
{
    private readonly Mock<IHtmlToPdfConverter> converter;
    private readonly Mock<IHtmlModelRenderer> htmlRenderer;
    private readonly ConfigurableHtmlToPdfModelRendererOptions<TestContentModel> options;
    private readonly ConfigurableHtmlToPdfModelRenderer<TestContentModel> renderer;

    public record TestContentModel();
    public record TestFooterModel();
    public record TestHeaderModel();

    public ConfigurableHtmlToPdfModelRendererTests()
    {
        htmlRenderer = new Mock<IHtmlModelRenderer>(MockBehavior.Strict);
        converter = new Mock<IHtmlToPdfConverter>(MockBehavior.Strict);
        options = new ConfigurableHtmlToPdfModelRendererOptions<TestContentModel>();
        renderer = new ConfigurableHtmlToPdfModelRenderer<TestContentModel>(htmlRenderer.Object, converter.Object, Options.Create(options));
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
        options.Margin = new(1, 2, 3, 4);

        htmlRenderer.Setup(m => m.RenderAsync(model, cancellationToken)).ReturnsAsync(contentResult).Verifiable();
        converter.Setup(m => m.ConvertAsync(It.Is<HtmlToPdfContext>(ctx =>
            ctx.Content == contentResult.Content &&
            ctx.FooterTemplate == null &&
            ctx.HeaderTemplate == null &&
            ctx.Margin == options.Margin), cancellationToken)
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

    [Fact]
    public async Task RenderAsync_RendersCorrectly_WithAFooterButNotHeader()
    {
        // arrange
        var model = new TestContentModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var fileId = Guid.NewGuid();
        var contentResult = new RenderResult<string>(Guid.NewGuid().ToString(), $"{fileId}.html");
        var converterResult = Mock.Of<Stream>(MockBehavior.Strict);
        var footerTemplate = Guid.NewGuid().ToString();
        options.Margin = new(1, 2, 3, 4);
        options.FooterTemplate = m => footerTemplate;

        htmlRenderer.Setup(m => m.RenderAsync(model, cancellationToken)).ReturnsAsync(contentResult).Verifiable();
        converter.Setup(m => m.ConvertAsync(It.Is<HtmlToPdfContext>(ctx =>
            ctx.Content == contentResult.Content &&
            ctx.FooterTemplate == footerTemplate &&
            ctx.HeaderTemplate == null &&
            ctx.Margin == options.Margin), cancellationToken)
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
    public async Task RenderAsync_RendersCorrectly_WithAFooterViewModelButNotHeader()
    {
        // arrange
        var model = new TestContentModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var fileId = Guid.NewGuid();
        var contentResult = new RenderResult<string>(Guid.NewGuid().ToString(), $"{fileId}.html");
        var converterResult = Mock.Of<Stream>(MockBehavior.Strict);
        var footerTemplate = Guid.NewGuid().ToString();
        var footerModel = new TestFooterModel();
        options.Margin = new(1, 2, 3, 4);
        options.FooterViewModel = m => footerModel;

        htmlRenderer.Setup(m => m.RenderAsync(model, cancellationToken)).ReturnsAsync(contentResult).Verifiable();
        htmlRenderer.Setup(m => m.RenderAsync(footerModel as object, cancellationToken)).ReturnsAsync(new RenderResult<string>(footerTemplate, "aaa")).Verifiable();
        converter.Setup(m => m.ConvertAsync(It.Is<HtmlToPdfContext>(ctx =>
            ctx.Content == contentResult.Content &&
            ctx.FooterTemplate == footerTemplate &&
            ctx.HeaderTemplate == null &&
            ctx.Margin == options.Margin), cancellationToken)
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
    public async Task RenderAsync_RendersCorrectly_WithAHeaderAndFooter()
    {
        // arrange
        var model = new TestContentModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var fileId = Guid.NewGuid();
        var contentResult = new RenderResult<string>(Guid.NewGuid().ToString(), $"{fileId}.html");
        var converterResult = Mock.Of<Stream>(MockBehavior.Strict);
        var headerTemplate = Guid.NewGuid().ToString();
        var footerTemplate = Guid.NewGuid().ToString();
        options.Margin = new(1, 2, 3, 4);
        options.HeaderTemplate = m => headerTemplate;
        options.FooterTemplate = m => footerTemplate;

        htmlRenderer.Setup(m => m.RenderAsync(model, cancellationToken)).ReturnsAsync(contentResult).Verifiable();
        converter.Setup(m => m.ConvertAsync(It.Is<HtmlToPdfContext>(ctx =>
            ctx.Content == contentResult.Content &&
            ctx.FooterTemplate == footerTemplate &&
            ctx.HeaderTemplate == headerTemplate &&
            ctx.Margin == options.Margin), cancellationToken)
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
    public async Task RenderAsync_RendersCorrectly_WithAHeaderAndFooterViewModel()
    {
        // arrange
        var model = new TestContentModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var fileId = Guid.NewGuid();
        var contentResult = new RenderResult<string>(Guid.NewGuid().ToString(), $"{fileId}.html");
        var converterResult = Mock.Of<Stream>(MockBehavior.Strict);
        var headerTemplate = Guid.NewGuid().ToString();
        var headerModel = new TestHeaderModel();
        var footerTemplate = Guid.NewGuid().ToString();
        var footerModel = new TestFooterModel();
        options.Margin = new(1, 2, 3, 4);
        options.HeaderViewModel = m => headerModel;
        options.FooterViewModel = m => footerModel;

        htmlRenderer.Setup(m => m.RenderAsync(model, cancellationToken)).ReturnsAsync(contentResult).Verifiable();
        htmlRenderer.Setup(m => m.RenderAsync(headerModel as object, cancellationToken)).ReturnsAsync(new RenderResult<string>(headerTemplate, "aaa")).Verifiable();
        htmlRenderer.Setup(m => m.RenderAsync(footerModel as object, cancellationToken)).ReturnsAsync(new RenderResult<string>(footerTemplate, "aaa")).Verifiable();
        converter.Setup(m => m.ConvertAsync(It.Is<HtmlToPdfContext>(ctx =>
            ctx.Content == contentResult.Content &&
            ctx.FooterTemplate == footerTemplate &&
            ctx.HeaderTemplate == headerTemplate &&
            ctx.Margin == options.Margin), cancellationToken)
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
    public async Task RenderAsync_RendersCorrectly_WithAHeaderButNotFooter()
    {
        // arrange
        var model = new TestContentModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var fileId = Guid.NewGuid();
        var contentResult = new RenderResult<string>(Guid.NewGuid().ToString(), $"{fileId}.html");
        var converterResult = Mock.Of<Stream>(MockBehavior.Strict);
        var headerTemplate = Guid.NewGuid().ToString();
        options.Margin = new(1, 2, 3, 4);
        options.HeaderTemplate = m => headerTemplate;

        htmlRenderer.Setup(m => m.RenderAsync(model, cancellationToken)).ReturnsAsync(contentResult).Verifiable();
        converter.Setup(m => m.ConvertAsync(It.Is<HtmlToPdfContext>(ctx =>
            ctx.Content == contentResult.Content &&
            ctx.FooterTemplate == null &&
            ctx.HeaderTemplate == headerTemplate &&
            ctx.Margin == options.Margin), cancellationToken)
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
    public async Task RenderAsync_RendersCorrectly_WithAHeaderViewModelButNotFooter()
    {
        // arrange
        var model = new TestContentModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var fileId = Guid.NewGuid();
        var contentResult = new RenderResult<string>(Guid.NewGuid().ToString(), $"{fileId}.html");
        var converterResult = Mock.Of<Stream>(MockBehavior.Strict);
        var headerTemplate = Guid.NewGuid().ToString();
        var headerModel = new TestHeaderModel();
        options.Margin = new(1, 2, 3, 4);
        options.HeaderViewModel = m => headerModel;

        htmlRenderer.Setup(m => m.RenderAsync(model, cancellationToken)).ReturnsAsync(contentResult).Verifiable();
        htmlRenderer.Setup(m => m.RenderAsync(headerModel as object, cancellationToken)).ReturnsAsync(new RenderResult<string>(headerTemplate, "aaa")).Verifiable();
        converter.Setup(m => m.ConvertAsync(It.Is<HtmlToPdfContext>(ctx =>
            ctx.Content == contentResult.Content &&
            ctx.FooterTemplate == null &&
            ctx.HeaderTemplate == headerTemplate &&
            ctx.Margin == options.Margin), cancellationToken)
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
    public async Task RenderAsync_RendersCorrectly_WithoutAFooterOrHeader()
    {
        // arrange
        var model = new TestContentModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var fileId = Guid.NewGuid();
        var contentResult = new RenderResult<string>(Guid.NewGuid().ToString(), $"{fileId}.html");
        var converterResult = Mock.Of<Stream>(MockBehavior.Strict);
        options.Margin = new(1, 2, 3, 4);

        htmlRenderer.Setup(m => m.RenderAsync(model, cancellationToken)).ReturnsAsync(contentResult).Verifiable();
        converter.Setup(m => m.ConvertAsync(It.Is<HtmlToPdfContext>(ctx =>
            ctx.Content == contentResult.Content &&
            ctx.FooterTemplate == null &&
            ctx.HeaderTemplate == null &&
            ctx.Margin == options.Margin), cancellationToken)
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
}