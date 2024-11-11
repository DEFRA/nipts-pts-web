using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
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
using Moq.Protected;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Defra.PTS.Web.UI.UnitTests.Controllers
{
    [TestFixture]
    public class TravelDocumentControllerAcknowledgementTests
    {
        private readonly Mock<IValidationService> _mockValidationService = new();
        private readonly Mock<IMediator> _mockMediator = new();
        private readonly Mock<ILogger<TravelDocumentController>> _mockLogger = new();
        private readonly Mock<ISelectListLocaliser> _mockBreedHelper = new();
        private readonly Mock<IOptions<PtsSettings>> _mockPtsSettings = new();
        private Mock<TravelDocumentController> _travelDocumentController;
        private Mock<TravelDocumentViewModel> _travelDocumentViewModel;
        private readonly IStringLocalizer<SharedResource> _localizer;

        public TravelDocumentControllerAcknowledgementTests()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<SharedResource>(factory);
        }

        [SetUp]
        public void Setup()
        {
            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<HttpContext>(), Mock.Of<ITempDataProvider>());
            _travelDocumentController = new Mock<TravelDocumentController>(
                _mockValidationService.Object,
                _mockMediator.Object,
                _mockLogger.Object,
                _mockPtsSettings.Object,
                _mockBreedHelper.Object,
                _localizer)
            {
                CallBase = true
            };
            _travelDocumentController.Object.TempData = tempData;
            _travelDocumentViewModel = new Mock<TravelDocumentViewModel>();

            var mockHttpContext = new Mock<HttpContext>();

            mockHttpContext.Setup(ctx => ctx.Request.Headers["Referer"]).Returns(GetReferer());

            _travelDocumentController.Object.ControllerContext = new ControllerContext
            {
                HttpContext = mockHttpContext.Object
            };
        }

        private static string GetReferer()
        {
            return "aaa";
        }

        [Test]
        public void Acknowledgement_Returns_RedirectToAction_When_Application_NotInProgress()
        {
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
                .Returns(false);

            // Act
            var result = _travelDocumentController.Object.Acknowledgement() as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.PetKeeperUserDetails), result.ActionName);
        }        

        [Test]
        public void Acknowledgement_Returns_RedirectToAction_When_Page_Does_Not_Meet_PreConditions()
        {
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
               .Returns(true);

            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    IsCompleted = true,
                },
                PetMicrochip = new PetMicrochipViewModel
                {
                    IsCompleted = true,
                },
                PetMicrochipDate = new PetMicrochipDateViewModel
                {
                    IsCompleted = true,
                },
                PetSpecies = new PetSpeciesViewModel
                {
                    PetSpecies = PetSpecies.Cat,
                    IsCompleted = true,
                },
                PetBreed = new PetBreedViewModel
                {
                    IsCompleted = true,
                },
                PetName = new PetNameViewModel
                {
                    IsCompleted = true,
                },
                PetGender = new PetGenderViewModel
                {
                    IsCompleted = true,
                },
                PetColour = new PetColourViewModel
                {
                    IsCompleted = true,
                },
                PetAge = new PetAgeViewModel { IsCompleted = true, },
                PetFeature = new PetFeatureViewModel
                {
                    IsCompleted = true,
                },
                 Declaration = new DeclarationViewModel
                 {
                     IsCompleted = false,
                 }
            };

            _travelDocumentController.Setup(x => x.GetFormData(false))
                 .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.Acknowledgement() as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);            

        }

        [Test]
        public void Acknowledgement_Returns_ViewResult_When_Page_Meets_PreConditions()
        {
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
               .Returns(true);
            var mock = new Mock<BaseController>();

            


            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    IsCompleted = true,
                },
                PetMicrochip = new PetMicrochipViewModel
                {
                    IsCompleted = true,
                },
                PetMicrochipDate = new PetMicrochipDateViewModel
                {
                    IsCompleted = true,
                },
                PetSpecies = new PetSpeciesViewModel
                {
                    PetSpecies = PetSpecies.Cat,
                    IsCompleted = true,
                },
                PetBreed = new PetBreedViewModel
                {
                    IsCompleted = true,
                },
                PetName = new PetNameViewModel
                {
                    IsCompleted = true,
                },
                PetGender = new PetGenderViewModel
                {
                    IsCompleted = true,
                },
                PetColour = new PetColourViewModel
                {
                    IsCompleted = true,
                },
                PetAge = new PetAgeViewModel { IsCompleted = true, },
                PetFeature = new PetFeatureViewModel
                {
                    IsCompleted = true,
                },
                Declaration = new DeclarationViewModel
                {
                    IsCompleted = true,
                }
            };

            _travelDocumentController.Setup(x => x.GetFormData(false))
                 .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.Acknowledgement() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

    }

}
