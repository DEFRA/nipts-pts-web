namespace Defra.PTS.Web.CertificateGenerator.UnitTests.Services;

public class CertificatePdfGeneratorTests
{
    private readonly CertificatePdfGenerator<TestModel> generator;
    private readonly Mock<IPdfModelRenderer<TestModel>> renderer;

    public record TestModel();

    public CertificatePdfGeneratorTests()
    {
        renderer = new Mock<IPdfModelRenderer<TestModel>>(MockBehavior.Strict);
        generator = new CertificatePdfGenerator<TestModel>(renderer.Object);
    }

    [Fact]
    public async Task GenerateAsync_RendersTheModel_WhenGivenAModel()
    {
        // arrange
        var model = new TestModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var content = new Mock<Stream>(MockBehavior.Strict);
        var name = Guid.NewGuid().ToString();
        var rendered = new RenderResult<Stream>(content.Object, name);

        renderer.Setup(m => m.RenderAsync(model, cancellationToken)).ReturnsAsync(rendered).Verifiable();

        // act
        var result = await generator.GenerateAsync(model, cancellationToken);

        // assert
        _ = result.Should().NotBeNull();
        _ = result.MimeType.Should().Be("application/pdf");
        _ = result.Name.Should().Be(name);
        _ = result.Content.Should().BeSameAs(content.Object);

        renderer.Verify();
        content.Verify();
    }

    [Fact]
    public async Task GenerateAsync_RendersTheModel_WhenGivenNoModel()
    {
        // arrange
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var content = new Mock<Stream>(MockBehavior.Strict);
        var name = Guid.NewGuid().ToString();
        var rendered = new RenderResult<Stream>(content.Object, name);

        renderer.Setup(m => m.RenderAsync(null, cancellationToken)).ReturnsAsync(rendered).Verifiable();

        // act
        var result = await generator.GenerateAsync(null, cancellationToken);

        // assert
        _ = result.Should().NotBeNull();
        _ = result.MimeType.Should().Be("application/pdf");
        _ = result.Name.Should().Be(name);
        _ = result.Content.Should().BeSameAs(content.Object);

        renderer.Verify();
        content.Verify();
    }
}