namespace Defra.PTS.Web.CertificateGenerator.UnitTests.Services;

public class CompositeCertificateGeneratorTests
{
    private readonly CompositeCertificateGenerator generator;
    private readonly Mock<IServiceProvider> services;

    public record TestModel();

    public CompositeCertificateGeneratorTests()
    {
        services = new Mock<IServiceProvider>(MockBehavior.Strict);
        generator = new CompositeCertificateGenerator(services.Object);
    }

    [Fact]
    public async Task GenerateAsync_RendersTheModel_WhenGivenAnUntypedModel()
    {
        // arrange
        var baseGenerator = new Mock<ICertificateGenerator<TestModel>>(MockBehavior.Strict);
        var model = new TestModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var expected = new CertificateResult(Guid.NewGuid().ToString(), Mock.Of<Stream>(MockBehavior.Strict), Guid.NewGuid().ToString());

        services.Setup(m => m.GetService(typeof(ICertificateGenerator<TestModel>))).Returns(baseGenerator.Object).Verifiable();
        baseGenerator.Setup(m => m.GenerateAsync(model, cancellationToken)).ReturnsAsync(expected).Verifiable();

        // act
        var result = await generator.GenerateAsync(model as object, cancellationToken);

        // assert
        _ = result.Should().BeSameAs(expected);

        services.Verify();
        baseGenerator.Verify();
    }

    [Fact]
    public async Task GenerateAsync_RendersTheModel_WhenGivenATypedModel()
    {
        // arrange
        var baseGenerator = new Mock<ICertificateGenerator<TestModel>>(MockBehavior.Strict);
        var model = new TestModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var expected = new CertificateResult(Guid.NewGuid().ToString(), Mock.Of<Stream>(MockBehavior.Strict), Guid.NewGuid().ToString());

        services.Setup(m => m.GetService(typeof(ICertificateGenerator<TestModel>))).Returns(baseGenerator.Object).Verifiable();
        baseGenerator.Setup(m => m.GenerateAsync(model, cancellationToken)).ReturnsAsync(expected).Verifiable();

        // act
        var result = await generator.GenerateAsync(model, cancellationToken);

        // assert
        _ = result.Should().BeSameAs(expected);

        services.Verify();
        baseGenerator.Verify();
    }
}