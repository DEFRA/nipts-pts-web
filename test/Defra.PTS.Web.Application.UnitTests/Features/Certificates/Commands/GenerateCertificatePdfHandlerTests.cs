using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Features.Certificates.Commands;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Models;
using Defra.PTS.Web.CertificateGenerator.ViewModels;
using Microsoft.Extensions.Logging;
using Moq;

namespace Defra.PTS.Web.Application.UnitTests.Features.Certificates.Commands;

public class GenerateCertificatePdfHandlerTests
{
    [Fact]
    public async Task Handle_ReturnsSuccessResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var applicationId = Guid.NewGuid();
        var applicationServiceMock = new Mock<IApplicationService>();
        var certificateGeneratorMock = new Mock<ICertificateGenerator>();
        var loggerMock = new Mock<ILogger<GenerateCertificatePdfHandler>>();

        var handler = new GenerateCertificatePdfHandler(
            applicationServiceMock.Object,
            certificateGeneratorMock.Object,
            loggerMock.Object);

        var request = new GenerateCertificatePdfRequest(applicationId, userId);

        var cancellationToken = CancellationToken.None;

        var applicationDetails = new ApplicationCertificateDto() { UserId = userId }; // Assuming some application certificate
        applicationServiceMock
            .Setup(x => x.GetApplicationCertificate(applicationId))
            .ReturnsAsync(applicationDetails);

        var certificateResult = new CertificateResult("Mock.pdf", new MemoryStream(), AppConstants.ContentTypes.Pdf);
        certificateGeneratorMock
            .Setup(m => m.GenerateAsync(It.IsAny<ApplicationCertificateViewModel>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(certificateResult);

        // Act
        var response = await handler.Handle(request, cancellationToken);

        // Assert
        Assert.Equal("Mock.pdf", response.Name);
        Assert.Equal(AppConstants.ContentTypes.Pdf, response.MimeType);
    }

    [Fact]
    public async Task Handle_ReturnsNullResponse()
    {
        // Arrange
        var userId = Guid.NewGuid();
        var applicationId = Guid.NewGuid();
        var applicationServiceMock = new Mock<IApplicationService>();
        var certificateGeneratorMock = new Mock<ICertificateGenerator>();
        var loggerMock = new Mock<ILogger<GenerateCertificatePdfHandler>>();

        var handler = new GenerateCertificatePdfHandler(
            applicationServiceMock.Object,
            certificateGeneratorMock.Object,
            loggerMock.Object);

        var request = new GenerateCertificatePdfRequest(applicationId, userId);

        var cancellationToken = CancellationToken.None;

        var applicationDetails = new ApplicationCertificateDto() { UserId = userId }; // Assuming some application details
        applicationServiceMock
            .Setup(x => x.GetApplicationCertificate(applicationId))
            .ReturnsAsync(applicationDetails);

        var certificateResult = new CertificateResult("Mock.pdf", new MemoryStream(), AppConstants.ContentTypes.Pdf);
        certificateGeneratorMock
            .Setup(m => m.GenerateAsync(It.IsAny<ApplicationCertificateViewModel>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new Exception());

        // Act
        var response = await handler.Handle(request, cancellationToken);

        // Assert
        Assert.Null(response);
        loggerMock.Verify();
    }
}
