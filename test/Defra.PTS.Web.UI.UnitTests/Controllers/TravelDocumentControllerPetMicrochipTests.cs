using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Controllers;
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
    public class TravelDocumentControllerPetMicrochipTests
    {
        private readonly Mock<IValidationService> _mockValidationService = new();
        private readonly Mock<IMediator> _mockMediator = new();
        private readonly Mock<ILogger<TravelDocumentController>> _mockLogger = new();
        private readonly Mock<IOptions<PtsSettings>> _mockPtsSettings = new();
        private Mock<TravelDocumentController> _travelDocumentController;
        private Mock<TravelDocumentViewModel> _travelDocumentViewModel;


        [SetUp]
        public void Setup()
        {
            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<Microsoft.AspNetCore.Http.HttpContext>(), Mock.Of<ITempDataProvider>());              
            _travelDocumentController = new Mock<TravelDocumentController>(_mockValidationService.Object, _mockMediator.Object, _mockLogger.Object, _mockPtsSettings.Object)
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
        public void PetMicrochip_Returns_RedirectToAction_When_Application_NotInProgress()
        {
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
                .Returns(false);

            // Act
            var result = _travelDocumentController.Object.PetMicrochip() as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.Index), result.ActionName);
        }

        

        [Test]
        public void PetMicrochip_Returns_RedirectToAction_When_Page_Does_Not_Meet_PreConditions()
        {
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
               .Returns(true);

            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    IsCompleted = true
                },
                PetKeeperName = new PetKeeperNameViewModel
                {
                    IsCompleted = true,
                },
                PetKeeperPostcode = new PetKeeperPostcodeViewModel
                {
                    IsCompleted = true,
                },
                PetKeeperPhone = new PetKeeperPhoneViewModel
                {
                    IsCompleted = false,                    
                },
                
            };
            formData.PetKeeperUserDetails.PetOwnerDetailsRequired = true;
            _travelDocumentController.Setup(x => x.GetFormData(false))
                 .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetMicrochip() as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);            

        }

        [Test]
        public void PetMicrochip_Returns_ViewResult_When_Page_Meets_PreConditions()
        {
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
               .Returns(true);

            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    IsCompleted = true
                },
                PetKeeperName = new PetKeeperNameViewModel
                {
                    IsCompleted = true,
                },
                PetKeeperPostcode = new PetKeeperPostcodeViewModel
                {
                    IsCompleted = true,
                },
                PetKeeperPhone = new PetKeeperPhoneViewModel
                {
                    IsCompleted = true,
                },

            };

            _travelDocumentController.Setup(x => x.GetFormData(false))
                 .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetMicrochip() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void PetMicrochip_Set_BackUrl_PetMicrochip_Returns_ViewResult_When_Page_Meets_PreConditions()
        {
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
               .Returns(true);

            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    IsCompleted = true
                },
                PetKeeperName = new PetKeeperNameViewModel
                {
                    IsCompleted = true,
                },
                PetKeeperPostcode = new PetKeeperPostcodeViewModel
                {
                    IsCompleted = true,
                },
                PetKeeperPhone = new PetKeeperPhoneViewModel
                {
                    IsCompleted = true,
                },
                PetMicrochipNotAvailable = new PetMicrochipNotAvailableViewModel { IsCompleted = true },

            };

            _travelDocumentController.Setup(x => x.GetFormData(false))
                 .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetMicrochipNotAvailable() as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(WebAppConstants.HistoryBack, result.ViewData[WebAppConstants.ViewKeys.BackUrl]);

        }

        [Test]
        public void PetMicrochip_WithValidModel_RedirectsToPetMicrochipNotAvailable()
        {
            // Arrange                                 
            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    IsCompleted = true
                },
                PetKeeperName = new PetKeeperNameViewModel
                {
                    IsCompleted = true,
                },
                PetKeeperPostcode = new PetKeeperPostcodeViewModel
                {
                    IsCompleted = true,
                },
                PetKeeperPhone = new PetKeeperPhoneViewModel
                {
                    IsCompleted = true,
                },
                PetMicrochipNotAvailable = new PetMicrochipNotAvailableViewModel { IsCompleted = true },
                

            };

            formData.PetMicrochip.Microchipped = YesNoOptions.No;
            _travelDocumentController.Setup(x => x.GetFormData(false))
                .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetMicrochip(formData.PetMicrochip) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.PetMicrochipNotAvailable), result.ActionName);
        }

        [Test]
        public void PetMicrochip_WithValidModel_RedirectsToPetMicrochipDate()
        {
            // Arrange                                 
            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    IsCompleted = true
                },
                PetKeeperName = new PetKeeperNameViewModel
                {
                    IsCompleted = true,
                },
                PetKeeperPostcode = new PetKeeperPostcodeViewModel
                {
                    IsCompleted = true,
                },
                PetKeeperPhone = new PetKeeperPhoneViewModel
                {
                    IsCompleted = true,
                },
                PetMicrochipNotAvailable = new PetMicrochipNotAvailableViewModel { IsCompleted = true },

            };
            formData.PetMicrochip.Microchipped = YesNoOptions.Yes;
            _travelDocumentController.Setup(x => x.GetFormData(false))
                .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetMicrochip(formData.PetMicrochip) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.PetMicrochipDate), result.ActionName);
        }


    }

}
