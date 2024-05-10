using Defra.PTS.Web.Application.DTOs.Features;
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
    public  class GetUserDetailQueryHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsGetUserDetailQueryResponse()
        {
            // Arrange
            Guid userId = Guid.NewGuid(); // Assuming some user id
            var userDetail = new UserDetailDto { /* Initialize user detail DTO */ };
            var response = new GetUserDetailQueryResponse { UserId = userId, UserDetail = userDetail };

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserDetail(userId)).ReturnsAsync(userDetail);

            var loggerMock = new Mock<ILogger<GetUserDetailQueryHandler>>();

            var handler = new GetUserDetailQueryHandler(userServiceMock.Object, loggerMock.Object);
            var request = new GetUserDetailQueryRequest(userId);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(userId, result.UserId);
            Assert.Equal(userDetail, result.UserDetail);
            // Add more assertions here based on your expectations for the result
        }

        [Fact]
        public async Task Handle_LogsErrorAndThrowsException_WhenUserServiceThrowsException()
        {
            // Arrange
            Guid userId = Guid.NewGuid(); // Assuming some user id
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.GetUserDetail(It.IsAny<Guid>())).ThrowsAsync(new Exception("Simulated error"));

            var loggerMock = new Mock<ILogger<GetUserDetailQueryHandler>>();

            var handler = new GetUserDetailQueryHandler(userServiceMock.Object, loggerMock.Object);
            var request = new GetUserDetailQueryRequest(userId);

            // Act + Assert
            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));
        }
    }
}
