using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Features.DynamicsCrm.Commands;
using Defra.PTS.Web.Application.Features.Users.Queries;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Controllers;
using Defra.PTS.Web.UI.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Security.Claims;
using Assert = NUnit.Framework.Assert;

namespace Defra.PTS.Web.UI.UnitTests.Controllers
{
    [TestFixture]
    public class TravelDocumentControllerPetKeeperUserDetailsTests
    {
        private readonly Mock<IValidationService> _mockValidationService = new();
        private readonly Mock<IMediator> _mockMediator = new();
        private readonly Mock<ILogger<TravelDocumentController>> _mockLogger = new();
        private readonly Mock<IOptions<PtsSettings>> _mockPtsSettings = new();
        private readonly Mock<ISelectListLocaliser> _mockBreedHelper = new();
        private IOptions<PtsSettings> _optionsPtsSettings;
        private Mock<TravelDocumentController> _travelDocumentController;
        private Mock<ControllerContext> _mockControllerContext;
        private Mock<TravelDocumentViewModel> _travelDocumentViewModel;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public TravelDocumentControllerPetKeeperUserDetailsTests()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<SharedResource>(factory);
        }

        [SetUp]
        public void Setup()
        {
            var ptsSettings = new PtsSettings
            {
                MagicWordEnabled = true,
            };
            _optionsPtsSettings = Options.Create(ptsSettings);
            _mockControllerContext = new Mock<ControllerContext>();
            _travelDocumentController = new Mock<TravelDocumentController>(_mockValidationService.Object, _mockMediator.Object, _mockLogger.Object, _optionsPtsSettings, _mockBreedHelper.Object, _localizer)
            {
                CallBase = true
            };
            _travelDocumentViewModel = new Mock<TravelDocumentViewModel>();

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(ctx => ctx.Request.Headers["Referer"]).Returns(GetReferer());
            _travelDocumentController.Object.ControllerContext = new ControllerContext();
            _travelDocumentController.Object.ControllerContext.HttpContext = mockHttpContext.Object;
        }

        private static string GetReferer()
        {
            return "aaa";
        }


        [Test]
        public void PetKeeperUserDetails_Returns_RedirectToAction_When_Application_NotInProgress()
        {
            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<Microsoft.AspNetCore.Http.HttpContext>(), Mock.Of<ITempDataProvider>());
            var magicWordViewModel = new MagicWordViewModel { HasUserPassedPasswordCheck = false };
            tempData.SetHasUserUsedMagicWord(magicWordViewModel);
            _travelDocumentController.Object.TempData = tempData;

            // Act
            var result = _travelDocumentController.Object.PetKeeperUserDetails().Result as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.Index), result.ActionName);
        }

        [Test]
        public void PetKeeperUserDetails_Returns_RedirectToAction_When_Page_PreConditions()
        {
            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<Microsoft.AspNetCore.Http.HttpContext>(), Mock.Of<ITempDataProvider>());
            var magicWordViewModel = new MagicWordViewModel { HasUserPassedPasswordCheck = true };
            tempData.SetHasUserUsedMagicWord(magicWordViewModel);
            _travelDocumentController.Object.TempData = tempData;
            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    Name = "John " + "Doe",
                    Email = "john.doe@example.com",
                    IsCompleted = true,
                    //UserDetailsAreCorrect = YesNoOptions.No,
                    PetOwnerDetailsRequired = false,
                },


            };
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
                .Returns(false);
            _travelDocumentController.Setup(x => x.GetFormData(false))
                .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetKeeperUserDetails(formData.PetKeeperUserDetails) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);

        }

        [Test]
        public void PetName_WithValidModel_If_UserDetailsAreCorrect_Yes_RedirectsToIndex()
        {
            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<Microsoft.AspNetCore.Http.HttpContext>(), Mock.Of<ITempDataProvider>());
            var magicWordViewModel = new MagicWordViewModel { HasUserPassedPasswordCheck = false };
            tempData.SetHasUserUsedMagicWord(magicWordViewModel);
            _travelDocumentController.Object.TempData = tempData;
            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    Name = "John " + "Doe",
                    Email = "john.doe@example.com",
                    IsCompleted = true,
                    UserDetailsAreCorrect = YesNoOptions.Yes,
                    PetOwnerDetailsRequired = false,
                },


            };
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
                .Returns(false);
            _travelDocumentController.Setup(x => x.GetFormData(false))
                .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetKeeperUserDetails(formData.PetKeeperUserDetails) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.Index), result.ActionName);
        }


        [Test]
        public void PetName_WithValidModel_If_UserDetailsAreCorrect_Yes_RedirectsToPetMicrochip()
        {
            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<Microsoft.AspNetCore.Http.HttpContext>(), Mock.Of<ITempDataProvider>());
            var magicWordViewModel = new MagicWordViewModel { HasUserPassedPasswordCheck = true };
            tempData.SetHasUserUsedMagicWord(magicWordViewModel);
            _travelDocumentController.Object.TempData = tempData;
            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    Name = "John " + "Doe",
                    Email = "john.doe@example.com",
                    IsCompleted = true,
                    UserDetailsAreCorrect = YesNoOptions.Yes,
                    PetOwnerDetailsRequired = false,
                },


            };
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
                .Returns(false);
            _travelDocumentController.Setup(x => x.GetFormData(false))
                .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetKeeperUserDetails(formData.PetKeeperUserDetails) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.PetMicrochip), result.ActionName);
        }

        [Test]
        public void PetName_WithValidModel_If_UserDetailsAreCorrect_Yes_RedirectsToPetKeeperName()
        {
            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<Microsoft.AspNetCore.Http.HttpContext>(), Mock.Of<ITempDataProvider>());
            var magicWordViewModel = new MagicWordViewModel { HasUserPassedPasswordCheck = true };
            tempData.SetHasUserUsedMagicWord(magicWordViewModel);
            _travelDocumentController.Object.TempData = tempData;
            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    Name = "John " + "Doe",
                    Email = "john.doe@example.com",
                    IsCompleted = true,
                    UserDetailsAreCorrect = YesNoOptions.No,
                    PetOwnerDetailsRequired = false,
                },


            };
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
                .Returns(false);
            _travelDocumentController.Setup(x => x.GetFormData(false))
                .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetKeeperUserDetails(formData.PetKeeperUserDetails) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.PetKeeperName), result.ActionName);
        }


        private void MockHttpContext()
        {
            // Arrange
            var mockHttpContext = new Mock<HttpContext>();
            var mockIdentity = new Mock<ClaimsIdentity>();
            var identities = new List<ClaimsIdentity>();
            // Create claims
            var claims = new List<Claim>
            {
                new Claim("contactId", "123"),
                new Claim("uniqueReference", "abc"),
                new Claim("firstName", "John"),
                new Claim("lastName", "Doe"),
                new Claim(ClaimTypes.Email, "john.doe@example.com"),
                new Claim(ClaimTypes.Role, "Admin")
            };


            identities.Add(mockIdentity.Object);
            // Setup ClaimsIdentity
            mockIdentity.SetupGet(i => i.Claims).Returns(claims);
            // Setup User
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims));


            mockHttpContext.Setup(c => c.User).Returns(user);
            // Setup Identities
            mockHttpContext.SetupGet(c => c.User.Identity).Returns(mockIdentity.Object);
            //mockHttpContext.SetupGet(c => c.User.Identity.IsAuthenticated).Returns(true);
            //mockHttpContext.SetupGet(c => c.User.GetLoggedInContactId()).Returns(new Guid());
            // Setup Identities
            mockHttpContext.SetupGet(c => c.User.Identities).Returns(identities);
            // Set up the behavior of GetHttpContext method on the controller
            _travelDocumentController.Setup(c => c.GetHttpContext()).Returns(mockHttpContext.Object);

        }



    }

}
