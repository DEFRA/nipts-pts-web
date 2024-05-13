namespace Defra.PTS.Web.CertificateGenerator.UnitTests.Services;

public class CompositeHtmlModelRendererTests
{
    private readonly CompositeHtmlModelRenderer generator;
    private readonly Mock<IServiceProvider> services;

    public record TestModel();

    public CompositeHtmlModelRendererTests()
    {
        services = new Mock<IServiceProvider>(MockBehavior.Strict);
        generator = new CompositeHtmlModelRenderer(services.Object);
    }

    [Fact]
    public async Task RenderAsync_RendersTheModel_WhenGivenAnUntypedModel()
    {
        // arrange
        var baseGenerator = new Mock<IHtmlModelRenderer<TestModel>>(MockBehavior.Strict);
        var model = new TestModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var expected = new RenderResult<string>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        services.Setup(m => m.GetService(typeof(IHtmlModelRenderer<TestModel>))).Returns(baseGenerator.Object).Verifiable();
        baseGenerator.Setup(m => m.RenderAsync(model, cancellationToken)).ReturnsAsync(expected).Verifiable();

        // act
        var result = await generator.RenderAsync(model as object, cancellationToken);

        // assert
        _ = result.Should().BeSameAs(expected);

        services.Verify();
        baseGenerator.Verify();
    }

    [Fact]
    public async Task RenderAsync_RendersTheModel_WhenGivenATypedModel()
    {
        // arrange
        var baseGenerator = new Mock<IHtmlModelRenderer<TestModel>>(MockBehavior.Strict);
        var model = new TestModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var expected = new RenderResult<string>(Guid.NewGuid().ToString(), Guid.NewGuid().ToString());

        services.Setup(m => m.GetService(typeof(IHtmlModelRenderer<TestModel>))).Returns(baseGenerator.Object).Verifiable();
        baseGenerator.Setup(m => m.RenderAsync(model, cancellationToken)).ReturnsAsync(expected).Verifiable();

        // act
        var result = await generator.RenderAsync(model, cancellationToken);

        // assert
        _ = result.Should().BeSameAs(expected);

        services.Verify();
        baseGenerator.Verify();
    }
}