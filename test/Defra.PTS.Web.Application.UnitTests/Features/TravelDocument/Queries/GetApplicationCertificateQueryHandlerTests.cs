using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Features.TravelDocument.Queries;
using Defra.PTS.Web.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Defra.PTS.Web.Application.UnitTests.Features.TravelDocument.Queries
{
    public class GetApplicationCertificateQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsGetApplicationCertificateQueryResponseWithCertificate()
        {
            // Arrange
            var applicationId = Guid.NewGuid(); // Assuming some application id
            var certificate = new ApplicationCertificateDto(); // Assuming some certificate

            var applicationServiceMock = new Mock<IApplicationService>();
            applicationServiceMock.Setup(x => x.GetApplicationCertificate(applicationId)).ReturnsAsync(certificate);

            var handler = new GetApplicationCertificateQueryHandler(applicationServiceMock.Object);
            var request = new GetApplicationCertificateQueryRequest(applicationId);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(applicationId, result.ApplicationId);
            Assert.NotNull(result.ApplicationCertificate);
            Assert.Same(certificate, result.ApplicationCertificate);
            // Add more assertions here based on your expectations for the result
        }

        [Fact]
        public async Task Handle_LogsErrorAndThrowsException_WhenApplicationServiceThrowsException()
        {
            // Arrange
            var applicationId = Guid.NewGuid(); // Assuming some application id
            string errorMessage = "Simulated error";

            var applicationServiceMock = new Mock<IApplicationService>();
            applicationServiceMock.Setup(x => x.GetApplicationCertificate(applicationId)).ThrowsAsync(new Exception(errorMessage));

            var handler = new GetApplicationCertificateQueryHandler(applicationServiceMock.Object);
            var request = new GetApplicationCertificateQueryRequest(applicationId );

            // Act + Assert
            var result = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));

            //Assert
            Assert.NotNull(result);
            Assert.Equal(errorMessage, result.Message);
        }
    }
}
