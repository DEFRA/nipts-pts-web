using Defra.PTS.Web.Application.DTOs.Services;
using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.CertificateGenerator.ViewModels;

[ExcludeFromCodeCoverage]
public class ApplicationDetailsViewModel : PdfViewModel
{
    public const string ViewName = "ApplicationDetails";
    public const string ViewPath = "Certificates";

    public ApplicationDetailsDto Data { get; set; }
}