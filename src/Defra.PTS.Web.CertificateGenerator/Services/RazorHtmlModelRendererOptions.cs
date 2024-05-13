using System;

namespace Defra.PTS.Web.CertificateGenerator.Services;

public class RazorHtmlModelRendererOptions<TModel>
{
    public Func<TModel, object> AdditionalViewData { get; set; }
    public Func<TModel, string> GetFileName { get; set; } = _ => $"{typeof(TModel).Name}.html";
    public string ViewName { get; set; }
    public string ViewPath { get; set; }
}