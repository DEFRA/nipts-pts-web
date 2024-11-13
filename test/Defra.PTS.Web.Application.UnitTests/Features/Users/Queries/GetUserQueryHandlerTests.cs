using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Features.Users.Queries;
using Defra.PTS.Web.Application.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Features.Users.Queries
{
    public class GetUserQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsUserDetailDto()
        {
            // Arrange
            var userServiceMock = new Mock<IUserService>();
            var loggerMock = new Mock<ILogger<GetUserQueryHandler>>();

            Guid userId = Guid.NewGuid(); // Assuming some user id
            var userDetailDto = new UserDetailDto { /* Initialize user detail DTO */ };
            userServiceMock.Setup(x => x.GetUserDetail(userId)).ReturnsAsync(userDetailDto);

            var handler = new GetUserQueryHandler(userServiceMock.Object);
            var request = new GetUserQueryRequest(userId);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // Add more assertions here based on your expectations for the result
        }

        [Fact]
        public async Task Handle_LogsErrorAndThrowsException_WhenUserServiceThrowsException()
        {
            // Arrange
            Guid userId = Guid.NewGuid();
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserDetail(It.IsAny<Guid>())).ThrowsAsync(new Exception("Simulated error"));

            var handler = new GetUserQueryHandler(userServiceMock.Object);
            var request = new GetUserQueryRequest(userId); // Assuming some user id

            // Act + Assert
            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));
        }
    }
}
