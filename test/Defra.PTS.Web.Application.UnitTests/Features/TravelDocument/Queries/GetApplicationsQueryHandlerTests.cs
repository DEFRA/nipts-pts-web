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
using static Defra.PTS.Web.Application.Constants.AppConstants;

namespace Defra.PTS.Web.Application.UnitTests.Features.TravelDocument.Queries
{
    public class GetApplicationsQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsGetApplicationsQueryResponseWithFilteredApplications()
        {
            // Arrange
            var userId = Guid.NewGuid(); // Assuming some user id
            var statuses = new List<string> { ApplicationStatus.AWAITINGVERIFICATION, ApplicationStatus.APPROVED }; // Assuming some statuses
            var applications = new List<ApplicationSummaryDto>
        {
            new ApplicationSummaryDto { ApplicationId = Guid.NewGuid(), Status = ApplicationStatus.AWAITINGVERIFICATION },
            new ApplicationSummaryDto { ApplicationId = Guid.NewGuid(), Status = ApplicationStatus.APPROVED },
            new ApplicationSummaryDto { ApplicationId = Guid.NewGuid(), Status = ApplicationStatus.REVOKED },
            new ApplicationSummaryDto { ApplicationId = Guid.NewGuid(), Status = ApplicationStatus.UNSUCCESSFUL },
            new ApplicationSummaryDto { ApplicationId = Guid.NewGuid(), Status = ApplicationStatus.SUSPENDED },
            new ApplicationSummaryDto { ApplicationId = Guid.NewGuid(), Status = null}
        }; // Assuming some applications

            var applicationServiceMock = new Mock<IApplicationService>();
            applicationServiceMock.Setup(x => x.GetUserApplications(userId)).ReturnsAsync(applications);

            var handler = new GetApplicationsQueryHandler(applicationServiceMock.Object);
            var request = new GetApplicationsQueryRequest(userId, statuses);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.NotNull(result.Applications);
            Assert.Equal(2, result.Applications.Count);
            Assert.Contains(result.Applications, x => x.Status == ApplicationStatus.AWAITINGVERIFICATION);
            Assert.Contains(result.Applications, x => x.Status == ApplicationStatus.APPROVED);
            Assert.Contains(result.Applications, x => x.Status == ApplicationStatus.SUSPENDED);

        }

        [Fact]
        public async Task Handle_LogsErrorAndThrowsException_WhenApplicationServiceThrowsException()
        {
            // Arrange
            var userId = Guid.NewGuid(); // Assuming some user id
            string errorMessage = "Simulated error";
            var statuses = new List<string> { ApplicationStatus.AWAITINGVERIFICATION, ApplicationStatus.APPROVED }; // Assuming some statuses


            var applicationServiceMock = new Mock<IApplicationService>();
            applicationServiceMock.Setup(x => x.GetUserApplications(userId)).ThrowsAsync(new Exception(errorMessage));

            var handler = new GetApplicationsQueryHandler(applicationServiceMock.Object);
            var request = new GetApplicationsQueryRequest(userId, statuses );

            // Act + Assert
            var result = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));

            //Assert
            Assert.NotNull(result);
            Assert.Equal(errorMessage, result.Message);
        }
    }
}
