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

namespace Defra.PTS.Web.Application.UnitTests.Features.TravelDocument.Queries
{
    public class GetApplicationDetailsQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsGetApplicationDetailsQueryResponseWithApplicationDetails()
        {
            // Arrange
            var applicationId = Guid.NewGuid(); // Assuming some application id
            var applicationDetails = new ApplicationDetailsDto(); // Assuming some application details

            var applicationServiceMock = new Mock<IApplicationService>();
            applicationServiceMock.Setup(x => x.GetApplicationDetails(applicationId)).ReturnsAsync(applicationDetails);

            var loggerMock = new Mock<ILogger<GetApplicationDetailsQueryHandler>>();

            var handler = new GetApplicationDetailsQueryHandler(applicationServiceMock.Object, loggerMock.Object);
            var request = new GetApplicationDetailsQueryRequest(applicationId);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(applicationId, result.ApplicationId);
            Assert.NotNull(result.ApplicationDetails);
            Assert.Same(applicationDetails, result.ApplicationDetails);
            // Add more assertions here based on your expectations for the result
        }

        [Fact]
        public async Task Handle_LogsErrorAndThrowsException_WhenApplicationServiceThrowsException()
        {
            // Arrange
            var applicationId = Guid.NewGuid(); // Assuming some application id
            string errorMessage = "Simulated error";

            var applicationServiceMock = new Mock<IApplicationService>();
            applicationServiceMock.Setup(x => x.GetApplicationDetails(applicationId)).ThrowsAsync(new Exception(errorMessage));

            var loggerMock = new Mock<ILogger<GetApplicationDetailsQueryHandler>>();

            var handler = new GetApplicationDetailsQueryHandler(applicationServiceMock.Object, loggerMock.Object);
            var request = new GetApplicationDetailsQueryRequest(applicationId);

            // Act + Assert
            var result = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));
            
            //Assert
            Assert.NotNull(result);
            Assert.Equal(errorMessage, result.Message);
        }
    }
}
