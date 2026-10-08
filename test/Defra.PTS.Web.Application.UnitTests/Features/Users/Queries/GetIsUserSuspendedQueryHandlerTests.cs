using Defra.PTS.Web.Application.Features.Users.Queries;
using Defra.PTS.Web.Application.Services.Interfaces;
using Moq;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Features.Users.Queries
{
    public class GetIsUserSuspendedQueryHandlerTests
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task Handle_ReturnsSuspensionStatus(bool isSuspended)
        {
            // Arrange
            Guid contactId = Guid.NewGuid();

            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.IsUserSuspended(contactId)).ReturnsAsync(isSuspended);

            var handler = new GetIsUserSuspendedQueryHandler(userServiceMock.Object);
            var request = new GetIsUserSuspendedQueryRequest(contactId);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(isSuspended, result.IsSuspended);
        }

        [Fact]
        public async Task Handle_ThrowsException_WhenUserServiceThrowsException()
        {
            // Arrange
            Guid contactId = Guid.NewGuid();
            var userServiceMock = new Mock<IUserService>();
            userServiceMock.Setup(x => x.IsUserSuspended(It.IsAny<Guid>())).ThrowsAsync(new Exception("Simulated error"));

            var handler = new GetIsUserSuspendedQueryHandler(userServiceMock.Object);
            var request = new GetIsUserSuspendedQueryRequest(contactId);

            // Act + Assert
            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));
        }
    }
}
