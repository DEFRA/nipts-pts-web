using Defra.PTS.Web.Application.Helpers;
using Defra.PTS.Web.Domain.Enums;

namespace Defra.PTS.Web.Application.UnitTests.Helpers;

public class ApplicationHelperTests
{

    [Fact]
    public void BuildQRCodeUrl()
    {
        // Arrange
        var text = "MockText";

        // Act
        var response = ApplicationHelper.BuildQRCodeUrl(text);
        var result = response.Contains("base64");

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void BuildPdfDownloadFilename()
    {
        // Arrange
        string petName = "Test";
        var pdfType = PdfType.Certificate;
        var prefix = "pet-travel-document";
        var expected = $"{prefix.ToLower()}-{pdfType.ToString().ToLower()}-{petName}.pdf";

        // Act
        var response = ApplicationHelper.BuildPdfDownloadFilename(petName, pdfType, prefix);

        // Assert
        Assert.Contains(expected,response);
    }

    [Fact]
    public void BuildPdfDownloadUrl_ApplicationDetails()
    {
        // Arrange
        var id = Guid.NewGuid();
        string petName = "test";
        var pdfType = PdfType.Application;
        var expected = $"/TravelDocument/DownloadApplicationDetailsPdf/{id}/{petName}";

        // Act
        var response = ApplicationHelper.BuildPdfDownloadUrl(id, pdfType, petName);

        // Assert
        Assert.Contains(expected, response);
    }


    [Fact]
    public void BuildPdfDownloadUrl_Certificate()
    {
        // Arrange
        var id = Guid.NewGuid();
        string petName = "test";
        var pdfType = PdfType.Certificate;
        var expected = $"/TravelDocument/DownloadCertificatePdf/{id}/{petName}";

        // Act
        var response = ApplicationHelper.BuildPdfDownloadUrl(id, pdfType, petName);

        // Assert
        Assert.Contains(expected, response);
    }
}
