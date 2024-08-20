using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Controllers;
using Defra.PTS.Web.UI.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Defra.PTS.Web.UI.UnitTests.Controllers
{
    [TestFixture]
    public class TravelDocumentControllerPetSpeciesTests
    {
        private readonly Mock<IValidationService> _mockValidationService = new();
        private readonly Mock<IMediator> _mockMediator = new();
        private readonly Mock<ILogger<TravelDocumentController>> _mockLogger = new();
        private readonly Mock<IOptions<PtsSettings>> _mockPtsSettings = new();
        private readonly Mock<IBreedHelper> _mockBreedHelper = new();
        private Mock<TravelDocumentController> _travelDocumentController;
        private Mock<TravelDocumentViewModel> _travelDocumentViewModel;


        [SetUp]
        public void Setup()
        {
            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<Microsoft.AspNetCore.Http.HttpContext>(), Mock.Of<ITempDataProvider>());              
            _travelDocumentController = new Mock<TravelDocumentController>(_mockValidationService.Object, _mockMediator.Object, _mockLogger.Object, _mockPtsSettings.Object, _mockBreedHelper.Object)
            {
                
                CallBase = true
            };
            _travelDocumentController.Object.TempData = tempData;
            _travelDocumentViewModel = new Mock<TravelDocumentViewModel>();

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(_ => _.Request.Headers["Referer"]).Returns("aaa");
            _travelDocumentController.Object.ControllerContext = new ControllerContext();
            _travelDocumentController.Object.ControllerContext.HttpContext = mockHttpContext.Object;
        }


        [Test]
        public void PetSpecies_Returns_RedirectToAction_When_Application_NotInProgress()
        {
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
                .Returns(false);

            // Act
            var result = _travelDocumentController.Object.PetSpecies(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.Index), result.ActionName);
        }

        [Test]
        public void PetSpecies_Returns_ViewResult_When_Page_Meets_PreConditions()
        {
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
               .Returns(true);

            var formData = new TravelDocumentViewModel
            {
                PetSpecies = new PetSpeciesViewModel
                {
                    PetSpecies = PetSpecies.Dog
                }
            };

            _travelDocumentController.Setup(x => x.GetFormData(false))
                 .Returns(formData);
            
            // Act
            var result = _travelDocumentController.Object.PetSpecies(1) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("PetKeeperUserDetails", result.ActionName);
        }

        [Test]
        public void PetSpecies_Returns_RedirectToAction_When_Page_Does_Not_Meet_PreConditions()
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
                    PetSpecies = PetSpecies.Cat
                }
            };

            _travelDocumentController.Setup(x => x.GetFormData(false))
                 .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetSpecies(1) as ViewResult;

            // Assert
            Assert.IsNotNull(result);            

        }


        [Test]
        public void PetSpecies_WithValidModel_RedirectsToPetName()
        {
            // Arrange                                 
            var formData = new TravelDocumentViewModel
            {
                PetSpecies = new PetSpeciesViewModel
                {
                    PetSpecies = PetSpecies.Ferret
                }
            };
            _travelDocumentController.Setup(x => x.GetFormData(false))
                .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetSpecies(formData.PetSpecies) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.PetName), result.ActionName);
        }


        [Test]
        public void PetSpecies_WithValidModel_RedirectsToPetBreed()
        {
            // Arrange                                 
            var formData = new TravelDocumentViewModel
            {
                PetSpecies = new PetSpeciesViewModel
                {
                    PetSpecies = PetSpecies.Dog
                }
            };
            _travelDocumentController.Setup(x => x.GetFormData(false))
                .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetSpecies(formData.PetSpecies) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.PetBreed), result.ActionName);
        }
    }

}
