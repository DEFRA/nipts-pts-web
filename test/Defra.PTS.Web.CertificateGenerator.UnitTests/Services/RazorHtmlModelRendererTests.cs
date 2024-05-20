using Defra.PTS.Web.CertificateGenerator.RazorHtml;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Metadata;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;

namespace Defra.PTS.Web.CertificateGenerator.UnitTests.Services;

public class RazorHtmlModelRendererTests
{
    private readonly Mock<IHttpContextAccessor> contextAccessor;
    private readonly Mock<IModelMetadataProvider> metadataProvider;
    private readonly RazorHtmlModelRendererOptions<TestModel> options;
    private readonly RazorHtmlModelRenderer<TestModel> renderer;
    private readonly Mock<ITempDataDictionaryFactory> tempDataFactory;
    private readonly Mock<ICompositeViewEngine> viewEngine;
    public record TestModel();

    public RazorHtmlModelRendererTests()
    {
        viewEngine = new Mock<ICompositeViewEngine>(MockBehavior.Strict);
        contextAccessor = new Mock<IHttpContextAccessor>(MockBehavior.Strict);
        metadataProvider = new Mock<IModelMetadataProvider>(MockBehavior.Strict);
        tempDataFactory = new Mock<ITempDataDictionaryFactory>(MockBehavior.Strict);
        options = new RazorHtmlModelRendererOptions<TestModel>();

        renderer = new RazorHtmlModelRenderer<TestModel>(viewEngine.Object, contextAccessor.Object, metadataProvider.Object, tempDataFactory.Object, Options.Create(options));
    }

    [Fact]
    public async Task RenderAsync_RendersCorrectly_WhenTheViewCanBeFound()
    {
        // arrange
        var viewPath = Guid.NewGuid().ToString();
        var viewName = Guid.NewGuid().ToString();
        var additionalViewData = new { Test = 1234 };
        var model = new TestModel();
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var httpContext = new Mock<HttpContext>(MockBehavior.Strict);
        var actualViewName = Guid.NewGuid().ToString();
        var view = new Mock<IView>(MockBehavior.Strict);
        var metadataIdentity = ModelMetadataIdentity.ForType(typeof(ModelMetadata));
        var metadata = new Mock<ModelMetadata>(MockBehavior.Strict, metadataIdentity);
        var tempData = new Mock<ITempDataDictionary>(MockBehavior.Strict);
        var html = Guid.NewGuid().ToString();
        var fileName = Guid.NewGuid().ToString();
        var getFileName = new Mock<Func<TestModel, string>>(MockBehavior.Strict);
        var additionalViewDataFn = new Mock<Func<TestModel, object>>(MockBehavior.Strict);
        options.GetFileName = getFileName.Object;
        options.ViewName = viewName;
        options.ViewPath = viewPath;
        options.AdditionalViewData = additionalViewDataFn.Object;

        additionalViewDataFn.Setup(m => m(model)).Returns(additionalViewData).Verifiable();
        contextAccessor.Setup(m => m.HttpContext).Returns(httpContext.Object).Verifiable();
        object controller;
        viewEngine
            .Setup(m => m.FindView(
                It.Is<ActionContext>(ctx =>
                    ctx.HttpContext == httpContext.Object &&
                    ctx.RouteData.Values.TryGetValue("controller", out controller) &&
                    Equals(controller, viewPath) &&
                    ctx.ActionDescriptor != null),
                viewName,
                true
            ))
            .Returns(ViewEngineResult.Found(actualViewName, view.Object))
            .Verifiable();
        metadataProvider.Setup(m => m.GetMetadataForType(typeof(TestModel))).Returns(metadata.Object).Verifiable();
        tempDataFactory.Setup(m => m.GetTempData(httpContext.Object)).Returns(tempData.Object).Verifiable();
        object test;
        view
            .Setup(m => m.RenderAsync(
                It.Is<ViewContext>(ctx =>
                    ctx.HttpContext == httpContext.Object &&
                    ctx.RouteData.Values.TryGetValue("controller", out controller) &&
                    Equals(controller, viewPath) &&
                    ctx.ActionDescriptor != null &&
                    ctx.View == view.Object &&
                    ctx.ViewData.ModelMetadata == metadata.Object &&
                    ctx.ViewData.Count() == 1 &&
                    ctx.ViewData.TryGetValue("Test", out test) &&
                    Equals(test, 1234) &&
                    ctx.Writer != null)
            ))
            .Callback((ViewContext ctx) => ctx.Writer.Write(html))
            .Returns(Task.CompletedTask)
            .Verifiable();
        getFileName.Setup(m => m(model)).Returns(fileName).Verifiable();

        // act
        var result = await renderer.RenderAsync(model, cancellationToken);

        // assert
        _ = result.Should().NotBeNull();
        _ = result.Content.Should().Be(html);
        _ = result.Name.Should().Be(fileName);

        getFileName.Verify();
        contextAccessor.Verify();
        viewEngine.Verify();
        metadataProvider.Verify();
        tempDataFactory.Verify();
        metadata.Verify();
        tempData.Verify();
        view.Verify();
        httpContext.Verify();
    }

    [Fact]
    public async Task RenderAsync_ThrowError_WhenTheViewCannotBeFound()
    {
        // arrange
        var viewPath = Guid.NewGuid().ToString();
        var viewName = Guid.NewGuid().ToString();
        var model = new TestModel();
        var additionalViewData = new { Test = 1234 };
        using var cts = new CancellationTokenSource();
        var cancellationToken = cts.Token;
        var httpContext = new Mock<HttpContext>(MockBehavior.Strict);
        var actualViewName = Guid.NewGuid().ToString();
        var getFileName = new Mock<Func<TestModel, string>>(MockBehavior.Strict);
        var additionalViewDataFn = new Mock<Func<TestModel, object>>(MockBehavior.Strict);
        options.GetFileName = getFileName.Object;
        options.ViewName = viewName;
        options.ViewPath = viewPath;
        options.AdditionalViewData = additionalViewDataFn.Object;

        additionalViewDataFn.Setup(m => m(model)).Returns(additionalViewData).Verifiable();
        contextAccessor.Setup(m => m.HttpContext).Returns(httpContext.Object).Verifiable();
        object controller;
        viewEngine
            .Setup(m => m.FindView(
                It.Is<ActionContext>(ctx =>
                    ctx.HttpContext == httpContext.Object &&
                    ctx.RouteData.Values.TryGetValue("controller", out controller) &&
                    Equals(controller, viewPath) &&
                    ctx.ActionDescriptor != null),
                viewName,
                true
            ))
            .Returns(ViewEngineResult.NotFound(actualViewName, Enumerable.Empty<string>()))
            .Verifiable();

        // act
        var test = async () => await renderer.RenderAsync(model, cancellationToken);
        var result = await test.Should().ThrowAsync<InvalidOperationException>();
        var ex = result.Which;

        // assert
        _ = ex.Should().NotBeNull();
        _ = ex.Message.Should().Be($"Failed to locate the {viewPath} view!");

        contextAccessor.Verify();
        viewEngine.Verify();
        metadataProvider.Verify();
        tempDataFactory.Verify();
        getFileName.Verify();
        httpContext.Verify();
    }
}
