using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Defra.PTS.Web.CertificateGenerator.Models;
[ExcludeFromCodeCoverage]

public record CertificateResult(string Name, Stream Content, string MimeType);