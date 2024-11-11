﻿using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.Features.Address.Queries;
using Defra.PTS.Web.Application.Features.DynamicsCrm.Commands;
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
using NUnit.Framework;
using Assert = NUnit.Framework.Assert;

namespace Defra.PTS.Web.UI.UnitTests.Controllers
{
    [TestFixture]
    public class TravelDocumentControllerPetKeeperAddressTests
    {
        private readonly Mock<IValidationService> _mockValidationService = new();
        private readonly Mock<IMediator> _mockMediator = new();
        private readonly Mock<ILogger<TravelDocumentController>> _mockLogger = new();
        private readonly Mock<IOptions<PtsSettings>> _mockPtsSettings = new();
        private readonly Mock<ISelectListLocaliser> _mockBreedHelper = new();
        private Mock<TravelDocumentController> _travelDocumentController;
        private Mock<TravelDocumentViewModel> _travelDocumentViewModel;
        private readonly IStringLocalizer<SharedResource> _localizer;
        public TravelDocumentControllerPetKeeperAddressTests()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<SharedResource>(factory);
        }

        [SetUp]
        public void Setup()
        {
            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<Microsoft.AspNetCore.Http.HttpContext>(), Mock.Of<ITempDataProvider>());              
            _travelDocumentController = new Mock<TravelDocumentController>(_mockValidationService.Object, _mockMediator.Object, _mockLogger.Object, _mockPtsSettings.Object, _mockBreedHelper.Object, _localizer)
            {
                
                CallBase = true
            };
            _travelDocumentController.Object.TempData = tempData;
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
        public void PetKeeperAddress_Returns_RedirectToAction_When_Application_NotInProgress()
        {
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
                .Returns(false);

            // Act
            var result = _travelDocumentController.Object.PetKeeperAddress().Result as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.Index), result.ActionName);
        }

        

        [Test]
        public void PetKeeperAddress_Returns_RedirectToAction_When_Page_Does_Not_Meet_PreConditions()
        {
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
               .Returns(true);

            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    IsCompleted = true,
                    PetOwnerDetailsRequired = true,
                },
                PetKeeperName = new PetKeeperNameViewModel
                {
                    IsCompleted = false,
                },
                PetKeeperAddress = new PetKeeperAddressViewModel
                {
                    IsCompleted = false,
                    Address = "Test",
                    Postcode = "Test",
                    
                }
            };

            _travelDocumentController.Setup(x => x.GetFormData(false))
                 .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetKeeperAddress().Result as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);            

        }

        [Test]
        public void PetKeeperAddress_Returns_ViewResult_When_Page_Meets_PreConditions_Redirect_PetKeeperPostcode()
        {
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
               .Returns(true);

            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
              .Returns(true);

            _mockMediator.Setup(x => x.Send(It.IsAny<ValidateGreatBritianAddressRequest>(), CancellationToken.None))
                 .ReturnsAsync(true);

            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    IsCompleted = true,
                    PetOwnerDetailsRequired = false,
                },
                PetKeeperName = new PetKeeperNameViewModel
                {
                    IsCompleted = true,
                },
                PetKeeperAddress = new PetKeeperAddressViewModel
                {
                    IsCompleted = false,
                    Address = "Test",
                    Postcode = "Test",
                    PostcodeRegion = PostcodeRegion.NonGB
                },
                PetKeeperPostcode = new PetKeeperPostcodeViewModel
                {
                    IsCompleted = false,
                    Postcode = "RM16 2 NT"
                }
            };

            _travelDocumentController.Setup(x => x.GetFormData(false))
                 .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetKeeperAddress().Result as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.PetKeeperPostcode), result.ActionName);
        }

        [Test]
        public void PetKeeperAddress_Returns_ViewResult_When_Page_Meets_PreConditions()
        {
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
               .Returns(true);

            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
              .Returns(true);

            _mockMediator.Setup(x => x.Send(It.IsAny<ValidateGreatBritianAddressRequest>(), CancellationToken.None))
                 .ReturnsAsync(true);

            _mockMediator.Setup(x => x.Send(It.IsAny<AddressLookupRequest>(), CancellationToken.None))
               .ReturnsAsync(new AddressLookupResponse
               {
                   Addresses = new List<Address>()
                   {
                       new Address()
                       {
                           AddressLineOne = "Test",
                           Postcode = "Test"

                       }
                   }
               });

            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    IsCompleted = true,
                    PetOwnerDetailsRequired = false,
                },
                PetKeeperName = new PetKeeperNameViewModel
                {
                    IsCompleted = true,                    
                },
                PetKeeperAddress = new PetKeeperAddressViewModel
                {
                    IsCompleted = false,
                    Address = "Test",
                    Postcode = "Test",
                    PostcodeRegion = PostcodeRegion.GB
                },
                PetKeeperPostcode = new PetKeeperPostcodeViewModel
                {
                    IsCompleted = false,
                    Postcode = "RM16 2 NT"
                }
            };

            _travelDocumentController.Setup(x => x.GetFormData(false))
                 .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetKeeperAddress().Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsNotNull(_travelDocumentController.Object.ViewBag.AddressList);
        }

        [Test]
        public void PetKeeperAddress_WithValidModel_RedirectsToPetKeeperPhone()
        {
            // Arrange                                 
            var formData = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    IsCompleted = true,
                    PetOwnerDetailsRequired = true,
                },
                PetKeeperName = new PetKeeperNameViewModel
                {
                    IsCompleted = true,
                },
                PetKeeperAddress = new PetKeeperAddressViewModel
                {
                    IsCompleted = true,
                    Postcode = "RM16 2GT"
                    
                }

            };
            _travelDocumentController.Setup(x => x.GetFormData(false))
                .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetKeeperAddress(formData.PetKeeperAddress).Result as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.PetKeeperPhone), result.ActionName);
        }


    }

}
