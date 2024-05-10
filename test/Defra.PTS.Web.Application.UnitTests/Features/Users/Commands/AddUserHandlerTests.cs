using Defra.PTS.Web.Application.Features.Users.Commands;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Sdk;

namespace Defra.PTS.Web.Application.UnitTests.Features.Users.Commands
{
    public class AddUserHandlerTests
    {
        [Fact]
        public async Task Handle_ReturnsAddUserResponseWithSuccess()
        {
            // Arrange
            var user = new User(); // Assuming you have a User object
            var userId = Guid.NewGuid(); // Assuming some user id
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.AddUserAsync(user)).ReturnsAsync(userId);

            var loggerMock = new Mock<ILogger<AddUserHandler>>();

            var handler = new AddUserHandler(userServiceMock.Object, loggerMock.Object);
            var request = new AddUserRequest(user);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.True(result.IsSuccess);
            Assert.Equal(userId, result.UserId);
            // Add more assertions here based on your expectations for the result
        }

        [Fact]
        public async Task Handle_LogsErrorAndThrowsException_WhenUserServiceThrowsException()
        {
            // Arrange
            var user = new User(); // Assuming you have a User object
            string errorMessage = "Simulated error";
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.AddUserAsync(user)).ThrowsAsync(new Exception(errorMessage));

            var loggerMock = new Mock<ILogger<AddUserHandler>>();

            var handler = new AddUserHandler(userServiceMock.Object, loggerMock.Object);
            var request = new AddUserRequest(user);

            // Act + Assert
           var result =  await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));
            //Assert
            Assert.NotNull(result);
            Assert.Equal(errorMessage, result.Message);
        }
    }
}
