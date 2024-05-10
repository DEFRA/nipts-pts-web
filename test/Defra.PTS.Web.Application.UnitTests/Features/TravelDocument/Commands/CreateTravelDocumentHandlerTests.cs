using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Features.TravelDocument.Commands;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Features.TravelDocument.Commands
{
    public class CreateTravelDocumentHandlerTests
    {

        [Fact]
        public async Task Handle_ReturnsSuccessResponse()
        {
            // Arrange
            var applicationServiceMock = new Mock<IApplicationService>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var dynamicServiceMock = new Mock<IDynamicService>();
            var loggerMock = new Mock<ILogger<CreateTravelDocumentHandler>>();

            var handler = new CreateTravelDocumentHandler(
                applicationServiceMock.Object,
                petServiceMock.Object,
                userServiceMock.Object,
                dynamicServiceMock.Object,
                loggerMock.Object);

            var request = new CreateTravelDocumentRequest(new TravelDocumentViewModel(), new User());

            var cancellationToken = CancellationToken.None;

            var userId = Guid.NewGuid(); // Example userId
            var ownerId = Guid.NewGuid(); // Example ownerId
            var ownerAddressId = Guid.NewGuid(); // Example ownerAddressId
            var petId = Guid.NewGuid(); // Example petId
            var applicationId = Guid.NewGuid(); // Example applicationId

            userServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<string>())).ReturnsAsync(userId);
            userServiceMock.Setup(m => m.AddOwnerAsync(It.IsAny<User>(), It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerId);
            userServiceMock.Setup(m => m.AddAddressAsync(AddressType.Owner, It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerAddressId);
            petServiceMock.Setup(m => m.CreatePet(It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(petId);
            applicationServiceMock.Setup(m => m.CreateApplication(It.IsAny<ApplicationDto>())).ReturnsAsync(new ApplicationDto { Id = applicationId, ReferenceNumber = "123456" });

            // Act
            var response = await handler.Handle(request, cancellationToken);

            // Assert
            Assert.True(response.IsSuccess);
            Assert.Equal("123456", response.Reference);
        }


        [Fact]
        public async Task Handle_LogsErrorAndThrowsException_WhenUserServiceThrowsException()
        {
            var errorMessage = "Simulated error";
            var applicationServiceMock = new Mock<IApplicationService>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var dynamicServiceMock = new Mock<IDynamicService>();
            var loggerMock = new Mock<ILogger<CreateTravelDocumentHandler>>();

            var handler = new CreateTravelDocumentHandler(
                applicationServiceMock.Object,
                petServiceMock.Object,
                userServiceMock.Object,
                dynamicServiceMock.Object,
                loggerMock.Object);

            var request = new CreateTravelDocumentRequest(new TravelDocumentViewModel(), new User());

            var cancellationToken = CancellationToken.None;

            var userId = Guid.NewGuid(); // Example userId
            var ownerId = Guid.NewGuid(); // Example ownerId
            var ownerAddressId = Guid.NewGuid(); // Example ownerAddressId
            var petId = Guid.NewGuid(); // Example petId
            var applicationId = Guid.NewGuid(); // Example applicationId

            userServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<string>())).ThrowsAsync(new Exception(errorMessage));
            userServiceMock.Setup(m => m.AddOwnerAsync(It.IsAny<User>(), It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerId);
            userServiceMock.Setup(m => m.AddAddressAsync(AddressType.Owner, It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerAddressId);
            petServiceMock.Setup(m => m.CreatePet(It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(petId);
            applicationServiceMock.Setup(m => m.CreateApplication(It.IsAny<ApplicationDto>())).ReturnsAsync(new ApplicationDto { Id = applicationId, ReferenceNumber = "123456" });

            // Act + Assert
            var result = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));

            Assert.NotNull(result);
            Assert.Equal(errorMessage, result.Message);
        }


        [Fact]
        public async Task Handle_LogsErrorAndThrowsException_WhenApplicationServiceThrowsException()
        {
            var errorMessage = "Simulated error";
            var applicationServiceMock = new Mock<IApplicationService>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var dynamicServiceMock = new Mock<IDynamicService>();
            var loggerMock = new Mock<ILogger<CreateTravelDocumentHandler>>();

            var handler = new CreateTravelDocumentHandler(
                applicationServiceMock.Object,
                petServiceMock.Object,
                userServiceMock.Object,
                dynamicServiceMock.Object,
                loggerMock.Object);

            var request = new CreateTravelDocumentRequest(new TravelDocumentViewModel(), new User());

            var cancellationToken = CancellationToken.None;

            var userId = Guid.NewGuid(); // Example userId
            var ownerId = Guid.NewGuid(); // Example ownerId
            var ownerAddressId = Guid.NewGuid(); // Example ownerAddressId
            var petId = Guid.NewGuid(); // Example petId
            var applicationId = Guid.NewGuid(); // Example applicationId

            userServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<string>())).ReturnsAsync(userId);
            userServiceMock.Setup(m => m.AddOwnerAsync(It.IsAny<User>(), It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerId);
            userServiceMock.Setup(m => m.AddAddressAsync(AddressType.Owner, It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerAddressId);
            petServiceMock.Setup(m => m.CreatePet(It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(petId);
            applicationServiceMock.Setup(m => m.CreateApplication(It.IsAny<ApplicationDto>())).ThrowsAsync(new Exception(errorMessage));

            // Act + Assert
            var result = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));

            Assert.NotNull(result);
            Assert.Equal(errorMessage, result.Message);
        }


        [Fact]
        public async Task Handle_LogsErrorAndThrowsException_WhenDynamicServiceThrowsException()
        {
            var errorMessage = "Simulated error";
            var applicationServiceMock = new Mock<IApplicationService>();
            var petServiceMock = new Mock<IPetService>();
            var userServiceMock = new Mock<IUserService>();
            var dynamicServiceMock = new Mock<IDynamicService>();
            var loggerMock = new Mock<ILogger<CreateTravelDocumentHandler>>();

            var handler = new CreateTravelDocumentHandler(
                applicationServiceMock.Object,
                petServiceMock.Object,
                userServiceMock.Object,
                dynamicServiceMock.Object,
                loggerMock.Object);

            var request = new CreateTravelDocumentRequest(new TravelDocumentViewModel(), new User());

            var cancellationToken = CancellationToken.None;

            var userId = Guid.NewGuid(); // Example userId
            var ownerId = Guid.NewGuid(); // Example ownerId
            var ownerAddressId = Guid.NewGuid(); // Example ownerAddressId
            var petId = Guid.NewGuid(); // Example petId
            var applicationId = Guid.NewGuid(); // Example applicationId

            userServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<string>())).ReturnsAsync(userId);
            userServiceMock.Setup(m => m.AddOwnerAsync(It.IsAny<User>(), It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerId);
            userServiceMock.Setup(m => m.AddAddressAsync(AddressType.Owner, It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerAddressId);
            petServiceMock.Setup(m => m.CreatePet(It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(petId);
            applicationServiceMock.Setup(m => m.CreateApplication(It.IsAny<ApplicationDto>())).ReturnsAsync(new ApplicationDto { Id = applicationId, ReferenceNumber = "123456" });
            dynamicServiceMock.Setup(a => a.AddApplicationToQueueAsync(It.IsAny<ApplicationSubmittedMessage>())).ThrowsAsync(new Exception(errorMessage));

            // Act + Assert
            var result = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));

            Assert.NotNull(result);
            Assert.Equal(errorMessage, result.Message);
        }
    }
}
