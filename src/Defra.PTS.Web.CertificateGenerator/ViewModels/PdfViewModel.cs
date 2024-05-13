using Defra.PTS.Web.Application.DTOs.Services;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.CertificateGenerator.ViewModels;

[ExcludeFromCodeCoverage]
public class PdfViewModel
{
    public string Watermark { get; set; }
}