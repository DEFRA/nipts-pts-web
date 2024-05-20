using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.CertificateGenerator.Models;
[ExcludeFromCodeCoverage]
public record RazorViewModel<T>(string ViewPath, string ViewName, T Model, object AdditionalViewData = null);