using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Defra.PTS.Web.UI.Controllers;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Defra.PTS.Web.UI.UnitTests.Controllers
{
    public class HomeControllerTests
    {
        private IOptions<PtsSettings> _optionsPtsSettings;
        private Mock<HomeController> _homeController;


        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void HomeController_Index_Returns_RedirectToAction_Considering_MagicWord(bool passwordCheck)
        {
            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<Microsoft.AspNetCore.Http.HttpContext>(), Mock.Of<ITempDataProvider>());
            var magicWordViewModel = new MagicWordViewModel { HasUserPassedPasswordCheck = passwordCheck };
            tempData.SetHasUserUsedMagicWord(magicWordViewModel);

            // Arrange
            var ptsSettings = new PtsSettings
            {
                MagicWordEnabled = true,
            };
            _optionsPtsSettings = Options.Create(ptsSettings);
            _homeController = new Mock<HomeController>(_optionsPtsSettings)
            {
                CallBase = true
            };

            _homeController.Object.TempData = tempData;

            // Act
            var result = _homeController.Object.Index();

            // Assert
            Assert.NotNull(result);
        }

        [Theory]
        [InlineData("TEST")]
        [InlineData("FAIL")]
        public void HomeController_Returns_RedirectToAction_Considering_MagicWord(string magicWord)
        {
            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<Microsoft.AspNetCore.Http.HttpContext>(), Mock.Of<ITempDataProvider>());
            var magicWordViewModel = new MagicWordViewModel { HasUserPassedPasswordCheck = true };
            tempData.SetHasUserUsedMagicWord(magicWordViewModel);

            // Arrange
            var ptsSettings = new PtsSettings
            {
                MagicWordEnabled = true,
                MagicWord = "TEST"
            };
            _optionsPtsSettings = Options.Create(ptsSettings);
            _homeController = new Mock<HomeController>(_optionsPtsSettings)
            {
                CallBase = true
            };
            _homeController.Object.TempData = tempData;
            var _homePageViewModel = new HomePageViewModel { MagicWordEnabled = true, EnteredPassword = magicWord };

            // Act
            var result = _homeController.Object.SubmitMagicWord(_homePageViewModel);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void HomeController_Returns_ApplicationTimeout_View()
        {
            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<Microsoft.AspNetCore.Http.HttpContext>(), Mock.Of<ITempDataProvider>());
            var magicWordViewModel = new MagicWordViewModel { HasUserPassedPasswordCheck = true };
            tempData.SetHasUserUsedMagicWord(magicWordViewModel);

            // Arrange
            var ptsSettings = new PtsSettings
            {
                MagicWordEnabled = true,
            };
            _optionsPtsSettings = Options.Create(ptsSettings);
            _homeController = new Mock<HomeController>(_optionsPtsSettings)
            {
                CallBase = true
            };

            _homeController.Object.TempData = tempData;

            // Act
            var result = _homeController.Object.ApplicationTimeout();

            // Assert
            Assert.NotNull(result);
            Assert.IsType<ViewResult>(result);
        }
    }
}
