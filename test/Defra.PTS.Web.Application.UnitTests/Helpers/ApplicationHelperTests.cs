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

        // Assert
        Assert.True(response.Contains("base64"));
    }

    [Fact]
    public void BuildPdfDownloadFilename()
    {
        // Arrange
        var id = Guid.NewGuid();
        var pdfType = PdfType.Certificate;
        var prefix = "pet-travel-document";
        var expected = $"{prefix.ToLower()}-{pdfType.ToString().ToLower()}-{id}.pdf";

        // Act
        var response = ApplicationHelper.BuildPdfDownloadFilename(id, pdfType, prefix);

        // Assert
        Assert.True(expected == response);
    }

}
