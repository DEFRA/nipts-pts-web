using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.CertificateGenerator.Models;

[ExcludeFromCodeCoverage]

public record HtmlToPdfContext
{
    public string Content { get; set; }

    public string FooterTemplate { get; set; } = null;

    public string HeaderTemplate { get; set; } = null;

    public MarginSize Margin { get; set; }
}