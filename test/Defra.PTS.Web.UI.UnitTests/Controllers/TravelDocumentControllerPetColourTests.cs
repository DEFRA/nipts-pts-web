﻿using Defra.PTS.Web.Application.Features.Lookups.Queries;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.DTOs;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Controllers;
using Defra.PTS.Web.UI.Helpers;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using System.Globalization;
using Assert = NUnit.Framework.Assert;

namespace Defra.PTS.Web.UI.UnitTests.Controllers
{
    [TestFixture]
    public class TravelDocumentControllerPetColourTests
    {
        private readonly Mock<IValidationService> _mockValidationService = new();
        private readonly Mock<IMediator> _mockMediator = new();
        private readonly Mock<ILogger<TravelDocumentController>> _mockLogger = new();
        private readonly Mock<IOptions<PtsSettings>> _mockPtsSettings = new();
        private readonly Mock<ISelectListLocaliser> _mockSelectListLocaliser = new();
        private Mock<TravelDocumentController> _sut;
        private readonly IStringLocalizer<ISharedResource> _localizer;
        public TravelDocumentControllerPetColourTests()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<ISharedResource>(factory);
        }

        [SetUp]
        public void Setup()
        {
            _sut = new Mock<TravelDocumentController>(_mockValidationService.Object, _mockMediator.Object, _mockLogger.Object, _mockPtsSettings.Object, _mockSelectListLocaliser.Object, _localizer)
            {
                CallBase = true
            };

            var mockHttpContext = new Mock<HttpContext>();
            mockHttpContext.Setup(_ => _.Request.Headers.Referer).Returns("aaa");
            _sut.Object.ControllerContext = new ControllerContext();
            _sut.Object.ControllerContext.HttpContext = mockHttpContext.Object;
        }

        [Ignore("Needs fixes")]
        public async Task GetColoursView()
        {
            var petColours = new List<ColourDto>
            {
                new ColourDto { Id = "1", Name = "Brown" },
                new ColourDto { Id = "2", Name = "Other" }
            };
            var formData = new TravelDocumentViewModel
            {
                PetSpecies = new PetSpeciesViewModel
                {
                    PetSpecies = PetSpecies.Dog
                }
            };

            _sut.Setup(x => x.IsApplicationInProgress())
                .Returns(true);

            _sut.Setup(x => x.GetFormData(false))
                .Returns(formData);

            _mockMediator.Setup(x => x.Send(It.IsAny<GetColoursQueryRequest>(), CancellationToken.None))
                .ReturnsAsync(new Application.DTOs.Features.GetColoursQueryResponse
                {
                    Colours = petColours,
                    PetType = PetSpecies.Dog
                });

            _sut.Setup(x => x.SaveFormData(It.IsAny<PetColourViewModel>()))
                .Verifiable();

            var result = await _sut.Object.PetColour();
            var viewResult = result as ViewResult;

            Assert.IsNotNull(viewResult);
        }


        [Test]
        public async Task RedirectToIndex_If_ApplicationNotInProgress()
        {
            _sut.Setup(x => x.IsApplicationInProgress())
                .Returns(false);

            var result = await _sut.Object.PetColour();

            var redirectResult = result as RedirectToActionResult;

            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("Index", redirectResult.ActionName);
        }

        [Test]
        public async Task CreatePetColour()
        {
            var petColoursList = new List<ColourDto>
            {
                new ColourDto { Id = "1", Name = "Brown" },
                new ColourDto { Id = "2", Name = "Other" }
            };
            var selectedPetColour = new PetColourViewModel
            {
                PetColour = 1,                
                OtherColourID = 2,
                PetSpecies = PetSpecies.Dog,
            };
            var formData = new TravelDocumentViewModel
            {
                PetSpecies = new PetSpeciesViewModel
                {
                    PetSpecies = PetSpecies.Dog
                },
                PetColour = selectedPetColour,
            };

            _sut.Setup(x => x.GetFormData(false))
                .Returns(formData);
            _sut.Setup(x => x.GetCYACheck()).Returns(false);
            _mockSelectListLocaliser.Setup(x => x.GetPetColoursList(It.IsAny<PetSpecies>()))
               .ReturnsAsync(petColoursList);

            _sut.Setup(x => x.SaveFormData(It.IsAny<PetColourViewModel>()))
                .Verifiable();


            var result = await _sut.Object.PetColour(selectedPetColour);

            var redirectResult = result as RedirectToActionResult;

            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("PetFeature", redirectResult.ActionName);
        }

        [Test]
        public async Task CreatePetColour_Welsh()
        {
            // Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("cy-GB"); // Set culture to Welsh

            var petColoursList = new List<ColourDto>
            {
                new ColourDto { Id = "1", Name = "Brown" },
                new ColourDto { Id = "2", Name = "Other" }
            };
            var selectedPetColour = new PetColourViewModel
            {
                PetColour = 1,
                OtherColourID = 2,
                PetSpecies = PetSpecies.Dog,
            };
            var formData = new TravelDocumentViewModel
            {
                PetSpecies = new PetSpeciesViewModel
                {
                    PetSpecies = PetSpecies.Dog
                },
                PetColour = selectedPetColour,
            };

            _sut.Setup(x => x.GetFormData(false))
                .Returns(formData);
            _sut.Setup(x => x.GetCYACheck()).Returns(false);
            _mockSelectListLocaliser.Setup(x => x.GetPetColoursList(It.IsAny<PetSpecies>()))
               .ReturnsAsync(petColoursList);
            _mockSelectListLocaliser.Setup(x => x.GetPetColoursListWithoutLocalisation(It.IsAny<PetSpecies>()))
                .ReturnsAsync(petColoursList);

            _sut.Setup(x => x.SaveFormData(It.IsAny<PetColourViewModel>()))
                .Verifiable();


            var result = await _sut.Object.PetColour(selectedPetColour);

            var redirectResult = result as RedirectToActionResult;

            Assert.IsNotNull(redirectResult);
            Assert.AreEqual("PetFeature", redirectResult.ActionName);
        }

        [Test]
        public async Task CreatePetColour_InvalidModel()
        {
            var petColoursList = new List<ColourDto>
            {
                new ColourDto { Id = "1", Name = "Brown" },
                new ColourDto { Id = "2", Name = "Other" }
            };

            _sut.Object.ModelState.AddModelError("PetColourId", "Id does not exist");

            _mockSelectListLocaliser.Setup(x => x.GetPetColoursList(It.IsAny<PetSpecies>()))
              .ReturnsAsync(petColoursList);

            var result = await _sut.Object.PetColour(new PetColourViewModel());

            var ViewResult = result as ViewResult;

            Assert.IsNotNull(ViewResult);
        }
    }

}
