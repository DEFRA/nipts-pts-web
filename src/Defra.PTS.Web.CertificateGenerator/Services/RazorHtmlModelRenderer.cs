using Defra.PTS.Web.CertificateGenerator.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Defra.PTS.Web.CertificateGenerator.Services;

public class RazorHtmlModelRenderer<TModel> : IHtmlModelRenderer<TModel>
{
    private readonly IHttpContextAccessor contextAccessor;
    private readonly IModelMetadataProvider metadataProvider;
    private readonly IOptions<RazorHtmlModelRendererOptions<TModel>> options;
    private readonly ITempDataDictionaryFactory tempDataFactory;
    private readonly ICompositeViewEngine viewEngine;

    public RazorHtmlModelRenderer(
        ICompositeViewEngine viewEngine,
        IHttpContextAccessor contextAccessor,
        IModelMetadataProvider metadataProvider,
        ITempDataDictionaryFactory tempDataFactory,
        IOptions<RazorHtmlModelRendererOptions<TModel>> options)
    {
        this.viewEngine = viewEngine;
        this.contextAccessor = contextAccessor;
        this.metadataProvider = metadataProvider;
        this.tempDataFactory = tempDataFactory;
        this.options = options;
    }

    public async Task<RenderResult<string>> RenderAsync(TModel model, CancellationToken cancellationToken)
    {
        var optionsVal = options.Value;
        ArgumentNullException.ThrowIfNull(optionsVal.ViewPath);
        ArgumentNullException.ThrowIfNull(optionsVal.ViewName);

        var httpContext = contextAccessor.HttpContext ?? throw new InvalidOperationException("There is no http context currently");
        var fakeRoute = new RouteData { Values = { ["controller"] = optionsVal.ViewPath } };
        var actionContext = new ActionContext(httpContext, fakeRoute, new ActionDescriptor());
        var viewResult = viewEngine.FindView(actionContext, optionsVal.ViewName, true);
        if (!viewResult.Success)
        {
            throw new InvalidOperationException($"Failed to locate the {optionsVal.ViewPath} view!");
        }

        var viewData = new ViewDataDictionary<TModel>(metadataProvider, new ModelStateDictionary());
        foreach (var (key, value) in HtmlHelper.ObjectToDictionary(optionsVal.AdditionalViewData?.Invoke(model)))
        {
            viewData.Add(key, value);
        }

        var html = new StringBuilder();
        using var writer = new StringWriter(html);
        var tempData = tempDataFactory.GetTempData(httpContext);
        var context = new ViewContext(actionContext, viewResult.View, viewData, tempData, writer, new HtmlHelperOptions());

        viewData.Model = model;
        await viewResult.View.RenderAsync(context).ConfigureAwait(false);
        await writer.FlushAsync().ConfigureAwait(false);

        return new(html.ToString(), options.Value.GetFileName(model));
    }
}