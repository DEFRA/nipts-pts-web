using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.CertificateGenerator.Models;

[ExcludeFromCodeCoverage]
public record RenderResult<T>(T Content, string Name);