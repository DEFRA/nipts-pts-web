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
        var referenceNumber = "NW7549A";
        var expected = $"{referenceNumber}.pdf";

        // Act
        var response = ApplicationHelper.BuildPdfDownloadFilename(referenceNumber);

        // Assert
        Assert.Contains(expected,response);
    }

    [Fact]
    public void BuildPdfDownloadUrl_ApplicationDetails()
    {
        // Arrange
        var id = Guid.NewGuid();
        var referenceNumber = "NW7549A";
        var pdfType = PdfType.Application;
        var expected = $"/TravelDocument/DownloadApplicationDetailsPdf/{id}/{referenceNumber}";

        // Act
        var response = ApplicationHelper.BuildPdfDownloadUrl(id, referenceNumber, pdfType);

        // Assert
        Assert.Contains(expected, response);
    }


    [Fact]
    public void BuildPdfDownloadUrl_Certificate()
    {
        // Arrange
        var id = Guid.NewGuid();
        var referenceNumber = "NW7549A";
        var pdfType = PdfType.Certificate;
        var expected = $"/TravelDocument/DownloadCertificatePdf/{id}/{referenceNumber}";

        // Act
        var response = ApplicationHelper.BuildPdfDownloadUrl(id, referenceNumber, pdfType);

        // Assert
        Assert.Contains(expected, response);
    }

    [Fact]
    public void FormatPetTravelDocumentNumber_Empty()
    {
        // Arrange
        var referenceNumber = "";

        // Act
        var response = ApplicationHelper.FormatPetTravelDocumentNumber(referenceNumber);

        // Assert
        Assert.Contains(referenceNumber, response);
    }

    [Fact]
    public void FormatPetTravelDocumentNumber_Populated()
    {
        // Arrange
        var referenceNumber = "GB82654499B";
        var formattedNumber = "GB826 544 99B";

        // Act
        var response = ApplicationHelper.FormatPetTravelDocumentNumber(referenceNumber);

        // Assert
        Assert.Contains(formattedNumber, response);
    }
}
