using Defra.PTS.Web.CertificateGenerator.Models;
using System;

namespace Defra.PTS.Web.CertificateGenerator.Services;

public class ConfigurableHtmlToPdfModelRendererOptions<TModel>
{
    public Func<TModel, string> FooterTemplate { get; set; }
    public Func<TModel, object> FooterViewModel { get; set; }
    public Func<TModel, string> HeaderTemplate { get; set; }
    public Func<TModel, object> HeaderViewModel { get; set; }
    public MarginSize? Margin { get; set; }
}