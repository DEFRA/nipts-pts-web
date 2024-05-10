﻿using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Features.Lookups.Queries;
using Defra.PTS.Web.Application.Features.Users.Queries;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Controllers;
using MediatR;
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
    public class TravelDocumentControllerPetBreedTests
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
        }

        [Test]
        public void PetBreed_Returns_RedirectToAction_When_Application_NotInProgress()
        {
            // Arrange
            _travelDocumentController.Setup(x => x.IsApplicationInProgress())
                .Returns(false);

            // Act
            var result = _travelDocumentController.Object.PetBreed().Result as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.PetKeeperUserDetails), result.ActionName);
        }

        [Test]
        public void PetBreed_Returns_RedirectToAction_When_Page_Does_Not_Meet_PreConditions()
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
                    IsCompleted = false,
                }
              
            };

            _travelDocumentController.Setup(x => x.GetFormData(false))
                 .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetBreed().Result as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);

        }

        [Test]
        public void PetBreed_Returns_ViewResult_When_Page_Meets_PreConditions_PetBreed_Not_Matching()
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
                    BreedId = 2,
                    BreedName = "Test2",
                    IsCompleted = true,
                },

            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetBreedsQueryRequest>(), CancellationToken.None))
                 .ReturnsAsync(new GetBreedsQueryResponse
                 {
                    PetType=PetSpecies.Cat,
                    Breeds = new List<BreedDto> { new BreedDto { BreedId=1,BreedName="Test"} }
                 });

            _travelDocumentController.Setup(x => x.GetFormData(false))
                 .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetBreed().Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void PetBreed_Returns_ViewResult_When_Page_Meets_PreConditions_PetBreed_Matching_With_Additional_Info()
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
                    BreedId = 2,
                    BreedName = "Test",
                    BreedAdditionalInfo= "Test Add Info",
                    IsCompleted = true,
                },

            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetBreedsQueryRequest>(), CancellationToken.None))
                 .ReturnsAsync(new GetBreedsQueryResponse
                 {
                     PetType = PetSpecies.Cat,
                     Breeds = new List<BreedDto> { new BreedDto { BreedId = 1, BreedName = "Test" } }
                 });

            _travelDocumentController.Setup(x => x.GetFormData(false))
                 .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetBreed().Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void PetBreed_WithValidModel_If_BreedName_Matches_RedirectsToPetName()
        {
            // Arrange                                 
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
                    PetSpecies = PetSpecies.Dog,
                    IsCompleted = true,
                },
                PetBreed = new PetBreedViewModel
                {

                    BreedId = 0,
                    BreedName = "Test",
                    BreedAdditionalInfo = "Test Add Info",
                    PetSpecies= PetSpecies.Dog,
                    IsCompleted = true,
                },
                

            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetBreedsQueryRequest>(), CancellationToken.None))
               .ReturnsAsync(new GetBreedsQueryResponse
               {
                   PetType = PetSpecies.Cat,
                   Breeds = new List<BreedDto> { new BreedDto { BreedId = 1, BreedName = "Test" } }
               });

            _travelDocumentController.Setup(x => x.GetFormData(false))
                .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetBreed(formData.PetBreed).Result as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.PetName), result.ActionName);

        }

        [Test]
        public void PetBreed_WithValidModel_If_BreedName_Does_Not_Matches_RedirectsToPetName()
        {
            // Arrange                                 
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
                    PetSpecies = PetSpecies.Dog,
                    IsCompleted = true,
                },
                PetBreed = new PetBreedViewModel
                {

                    BreedId = 0,
                    BreedName = "Test3",
                    BreedAdditionalInfo = "Test Add Info",
                    PetSpecies = PetSpecies.Dog,
                    IsCompleted = true,
                },


            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetBreedsQueryRequest>(), CancellationToken.None))
               .ReturnsAsync(new GetBreedsQueryResponse
               {
                   PetType = PetSpecies.Cat,
                   Breeds = new List<BreedDto> { new BreedDto { BreedId = 1, BreedName = "Test" } }
               });

            _travelDocumentController.Setup(x => x.GetFormData(false))
                .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetBreed(formData.PetBreed).Result as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.PetName), result.ActionName);

        }

        [Test]
        public void PetBreed_WithValidModel_BreedIdEquatlTo300_If_BreedName_Matches_RedirectsToPetName()
        {
            // Arrange                                 
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
                    PetSpecies = PetSpecies.Dog,
                    IsCompleted = true,
                },
                PetBreed = new PetBreedViewModel
                {

                    BreedId = 300,
                    BreedName = "Test",
                    BreedAdditionalInfo = "Test Add Info",
                    PetSpecies = PetSpecies.Dog,
                    IsCompleted = true,
                },


            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetBreedsQueryRequest>(), CancellationToken.None))
               .ReturnsAsync(new GetBreedsQueryResponse
               {
                   PetType = PetSpecies.Cat,
                   Breeds = new List<BreedDto> { new BreedDto { BreedId = 1, BreedName = "Test" } }
               });

            _travelDocumentController.Setup(x => x.GetFormData(false))
                .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetBreed(formData.PetBreed).Result as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.PetName), result.ActionName);

        }

        [Test]
        public void PetBreed_WithValidModel__BreedIdEquatlTo300_If_BreedName_Does_Not_Matches_RedirectsToPetName()
        {
            // Arrange                                 
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
                    PetSpecies = PetSpecies.Dog,
                    IsCompleted = true,
                },
                PetBreed = new PetBreedViewModel
                {

                    BreedId = 0,
                    BreedName = "Test3",
                    BreedAdditionalInfo = "Test Add Info",
                    PetSpecies = PetSpecies.Dog,
                    IsCompleted = true,
                },


            };

            _mockMediator.Setup(x => x.Send(It.IsAny<GetBreedsQueryRequest>(), CancellationToken.None))
               .ReturnsAsync(new GetBreedsQueryResponse
               {
                   PetType = PetSpecies.Cat,
                   Breeds = new List<BreedDto> { new BreedDto { BreedId = 1, BreedName = "Test" } }
               });

            _travelDocumentController.Setup(x => x.GetFormData(false))
                .Returns(formData);

            // Act
            var result = _travelDocumentController.Object.PetBreed(formData.PetBreed).Result as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.PetName), result.ActionName);

        }


    }

}
