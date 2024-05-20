using Defra.PTS.Web.Application.DTOs.Services;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.CertificateGenerator.ViewModels;

[ExcludeFromCodeCoverage]
public class ApplicationCertificateViewModel : PdfViewModel
{
    public const string ViewName = "ApplicationCertificate";
    public const string ViewPath = "Certificates";

    public ApplicationCertificateDto Data { get; set; }
}