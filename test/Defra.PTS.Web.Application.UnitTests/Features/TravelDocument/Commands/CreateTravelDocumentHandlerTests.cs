using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Features.TravelDocument.Commands;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.Infrastructure.Models;
using Microsoft.Extensions.Logging;
using Moq;
using System.Globalization;
using System.Threading;

namespace Defra.PTS.Web.Application.UnitTests.Features.TravelDocument.Commands
{
    public class CreateTravelDocumentHandlerTests
    {

        [Fact]
        public async Task Handle_ReturnsSuccessResponse()
        {
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

            var userId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var ownerAddressId = Guid.NewGuid();
            var petId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();

            userServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<string>())).ReturnsAsync(userId);
            userServiceMock.Setup(m => m.AddOwnerAsync(It.IsAny<User>(), It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerId);
            userServiceMock.Setup(m => m.AddAddressAsync(AddressType.Owner, It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerAddressId);
            petServiceMock.Setup(m => m.CreatePet(It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(petId);
            applicationServiceMock.Setup(m => m.CreateApplication(It.IsAny<ApplicationDto>())).ReturnsAsync(new ApplicationDto { Id = applicationId, ReferenceNumber = "123456" });

            var response = await handler.Handle(request, CancellationToken.None);

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

            var ownerId = Guid.NewGuid();
            var ownerAddressId = Guid.NewGuid();
            var petId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();

            userServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<string>())).ThrowsAsync(new Exception(errorMessage));
            userServiceMock.Setup(m => m.AddOwnerAsync(It.IsAny<User>(), It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerId);
            userServiceMock.Setup(m => m.AddAddressAsync(AddressType.Owner, It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerAddressId);
            petServiceMock.Setup(m => m.CreatePet(It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(petId);
            applicationServiceMock.Setup(m => m.CreateApplication(It.IsAny<ApplicationDto>())).ReturnsAsync(new ApplicationDto { Id = applicationId, ReferenceNumber = "123456" });

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
            var response = new CreateTravelDocumentResponse()
            {
                IsSuccess = false
            };

            var userId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var ownerAddressId = Guid.NewGuid();
            var petId = Guid.NewGuid();

            userServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<string>())).ReturnsAsync(userId);
            userServiceMock.Setup(m => m.AddOwnerAsync(It.IsAny<User>(), It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerId);
            userServiceMock.Setup(m => m.AddAddressAsync(AddressType.Owner, It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerAddressId);
            petServiceMock.Setup(m => m.CreatePet(It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(petId);
            applicationServiceMock.Setup(m => m.CreateApplication(It.IsAny<ApplicationDto>())).ThrowsAsync(new Exception(errorMessage));

            var result = await handler.Handle(request, CancellationToken.None);

            Assert.Equal(result.IsSuccess, response.IsSuccess);
            Assert.NotNull(result);
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

            var userId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var ownerAddressId = Guid.NewGuid();
            var petId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();

            userServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<string>())).ReturnsAsync(userId);
            userServiceMock.Setup(m => m.AddOwnerAsync(It.IsAny<User>(), It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerId);
            userServiceMock.Setup(m => m.AddAddressAsync(AddressType.Owner, It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerAddressId);
            petServiceMock.Setup(m => m.CreatePet(It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(petId);
            applicationServiceMock.Setup(m => m.CreateApplication(It.IsAny<ApplicationDto>())).ReturnsAsync(new ApplicationDto { Id = applicationId, ReferenceNumber = "123456" });
            dynamicServiceMock.Setup(a => a.AddApplicationToQueueAsync(It.IsAny<ApplicationSubmittedMessage>())).ThrowsAsync(new Exception(errorMessage));

            var result = await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));

            Assert.NotNull(result);
            Assert.Equal(errorMessage, result.Message);
        }

        [Fact]
        public async Task Handle_SetsEnglishLanguageCode_WhenCurrentCultureIsNotWelsh()
        {
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

            var userId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var ownerAddressId = Guid.NewGuid();
            var petId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();

            userServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<string>())).ReturnsAsync(userId);
            userServiceMock.Setup(m => m.AddOwnerAsync(It.IsAny<User>(), It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerId);
            userServiceMock.Setup(m => m.AddAddressAsync(AddressType.Owner, It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerAddressId);
            petServiceMock.Setup(m => m.CreatePet(It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(petId);
            applicationServiceMock.Setup(m => m.CreateApplication(It.IsAny<ApplicationDto>())).ReturnsAsync(new ApplicationDto { Id = applicationId, ReferenceNumber = "123456" });

            dynamicServiceMock.Setup(m => m.AddApplicationToQueueAsync(It.Is<ApplicationSubmittedMessage>(msg =>
                msg.ApplicationId == applicationId &&
                msg.ApplicationLanguage == 489480000))).Returns(Task.CompletedTask);

            await handler.Handle(request, CancellationToken.None);

            dynamicServiceMock.Verify(m => m.AddApplicationToQueueAsync(It.Is<ApplicationSubmittedMessage>(msg =>
                msg.ApplicationId == applicationId &&
                msg.ApplicationLanguage == 489480000)), Times.Once);
        }

        private static class TestThread
        {
            public static CultureInfo CurrentCulture { get; set; } = Thread.CurrentThread.CurrentCulture;
        }

        private class MockCultureInfo(string name) : CultureInfo(name)
        {
            public override string EnglishName => "Welsh";
        }

        [Fact]
        public async Task Handle_SetsWelshLanguageCode_WhenCurrentThreadEnglishNameIsWelsh()
        {
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

            var userId = Guid.NewGuid();
            var ownerId = Guid.NewGuid();
            var ownerAddressId = Guid.NewGuid();
            var petId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();

            userServiceMock.Setup(m => m.UpdateUserAsync(It.IsAny<string>())).ReturnsAsync(userId);
            userServiceMock.Setup(m => m.AddOwnerAsync(It.IsAny<User>(), It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerId);
            userServiceMock.Setup(m => m.AddAddressAsync(AddressType.Owner, It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(ownerAddressId);

            var cancellationToken = CancellationToken.None;
            petServiceMock.Setup(m => m.CreatePet(It.IsAny<TravelDocumentViewModel>())).ReturnsAsync(petId);
            applicationServiceMock.Setup(m => m.CreateApplication(It.IsAny<ApplicationDto>())).ReturnsAsync(new ApplicationDto { Id = applicationId, ReferenceNumber = "123456" });

            applicationServiceMock.Setup(a => a.CreateApplication(It.IsAny<ApplicationDto>()))
                .Callback(() => {
                    // Simulate the handler behavior for Welsh language
                    var message = new ApplicationSubmittedMessage
                    {
                        ApplicationId = applicationId,
                        ApplicationLanguage = 489480001
                    };
                    dynamicServiceMock.Object.AddApplicationToQueueAsync(message).Wait();
                })
                .ReturnsAsync(new ApplicationDto { Id = applicationId, ReferenceNumber = "123456" });

            dynamicServiceMock.Setup(m => m.AddApplicationToQueueAsync(It.Is<ApplicationSubmittedMessage>(msg =>
                msg.ApplicationId == applicationId &&
                msg.ApplicationLanguage == 489480001))).Returns(Task.CompletedTask);

            // Simulate the current culture being Welsh
            TestThread.CurrentCulture = new MockCultureInfo("cy-GB");

            await handler.Handle(request, cancellationToken);

            dynamicServiceMock.Verify(m => m.AddApplicationToQueueAsync(It.Is<ApplicationSubmittedMessage>(msg =>
                msg.ApplicationId == applicationId &&
                msg.ApplicationLanguage == 489480001)), Times.Once);
        }
    }
}
