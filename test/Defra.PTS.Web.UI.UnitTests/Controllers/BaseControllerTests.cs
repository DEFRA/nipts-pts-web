using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Domain.ViewModels;
using Defra.PTS.Web.UI.Constants;
using Defra.PTS.Web.UI.Controllers;
using Defra.PTS.Web.UI.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Primitives;
using Moq;
using System.Collections.Generic;
using System.Globalization;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading;
using Xunit;

namespace Defra.PTS.Web.UI.UnitTests.Controllers
{
    public class BaseControllerTests
    {
        private readonly ConcreteBaseController _controller;
        private readonly Mock<HttpContext> _mockHttpContext;
        private readonly Mock<ISession> _mockSession;
        private readonly Mock<IQueryCollection> _mockQueryCollection;
        private readonly TempDataDictionary _tempData;

        public BaseControllerTests()
        {
            _mockHttpContext = new Mock<HttpContext>();
            _mockSession = new Mock<ISession>();
            _mockQueryCollection = new Mock<IQueryCollection>();
            _tempData = new TempDataDictionary(_mockHttpContext.Object, Mock.Of<ITempDataProvider>());

            // Setup HttpContext
            _mockHttpContext.Setup(x => x.Session).Returns(_mockSession.Object);
            _mockHttpContext.Setup(x => x.Request.Query).Returns(_mockQueryCollection.Object);

            // Create concrete controller instance
            _controller = new ConcreteBaseController(_mockHttpContext.Object);
            _controller.ControllerContext = new ControllerContext
            {
                HttpContext = _mockHttpContext.Object
            };
            _controller.TempData = _tempData;
        }

        [Fact]
        public void OnActionExecuting_WithCyParameter_SetsWelshCulture()
        {
            // Arrange
            _mockQueryCollection.Setup(x => x.ContainsKey("cy")).Returns(true);
            _mockQueryCollection.Setup(x => x.ContainsKey("en")).Returns(false);

            _mockSession.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<byte[]>()));

            var actionExecutingContext = CreateActionExecutingContext();

            // Act
            _controller.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Equal("cy", Thread.CurrentThread.CurrentCulture.Name);
            Assert.Equal("cy", Thread.CurrentThread.CurrentUICulture.Name);
            _mockSession.Verify(x => x.Set("Language", It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void OnActionExecuting_WithEnParameter_SetsEnglishCulture()
        {
            // Arrange
            _mockQueryCollection.Setup(x => x.ContainsKey("en")).Returns(true);
            _mockQueryCollection.Setup(x => x.ContainsKey("cy")).Returns(false);

            _mockSession.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<byte[]>()));

            var actionExecutingContext = CreateActionExecutingContext();

            // Act
            _controller.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Equal("en-GB", Thread.CurrentThread.CurrentCulture.Name);
            Assert.Equal("en-GB", Thread.CurrentThread.CurrentUICulture.Name);
            _mockSession.Verify(x => x.Set("Language", It.IsAny<byte[]>()), Times.Once);
        }

        [Fact]
        public void OnActionExecuting_WithSavedWelshLanguage_SetsWelshCulture()
        {
            // Arrange
            _mockQueryCollection.Setup(x => x.ContainsKey("cy")).Returns(false);
            _mockQueryCollection.Setup(x => x.ContainsKey("en")).Returns(false);

            var savedLanguage = "cy";
            var sessionValue = System.Text.Encoding.UTF8.GetBytes(savedLanguage);
            _mockSession.Setup(x => x.TryGetValue("Language", out sessionValue)).Returns(true);

            var actionExecutingContext = CreateActionExecutingContext();

            // Act
            _controller.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Equal("cy", Thread.CurrentThread.CurrentCulture.Name);
            Assert.Equal("cy", Thread.CurrentThread.CurrentUICulture.Name);
        }

        [Fact]
        public void OnActionExecuting_WithSavedEnglishLanguage_SetsEnglishCulture()
        {
            // Arrange
            _mockQueryCollection.Setup(x => x.ContainsKey("cy")).Returns(false);
            _mockQueryCollection.Setup(x => x.ContainsKey("en")).Returns(false);

            var savedLanguage = "en-GB";
            var sessionValue = System.Text.Encoding.UTF8.GetBytes(savedLanguage);
            _mockSession.Setup(x => x.TryGetValue("Language", out sessionValue)).Returns(true);

            var actionExecutingContext = CreateActionExecutingContext();

            // Act
            _controller.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Equal("en-GB", Thread.CurrentThread.CurrentCulture.Name);
            Assert.Equal("en-GB", Thread.CurrentThread.CurrentUICulture.Name);
        }

        [Fact]
        public void OnActionExecuting_WithNoLanguageParameters_DoesNotChangeCulture()
        {
            // Arrange
            var originalCulture = Thread.CurrentThread.CurrentCulture;
            var originalUICulture = Thread.CurrentThread.CurrentUICulture;

            _mockQueryCollection.Setup(x => x.ContainsKey("cy")).Returns(false);
            _mockQueryCollection.Setup(x => x.ContainsKey("en")).Returns(false);

            var sessionValue = (byte[])null;
            _mockSession.Setup(x => x.TryGetValue("Language", out sessionValue)).Returns(false);

            var actionExecutingContext = CreateActionExecutingContext();

            // Act
            _controller.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Equal(originalCulture, Thread.CurrentThread.CurrentCulture);
            Assert.Equal(originalUICulture, Thread.CurrentThread.CurrentUICulture);
        }

        [Fact]
        public void OnActionExecuting_CyParameterTakesPrecedenceOverSavedLanguage()
        {
            // Arrange
            _mockQueryCollection.Setup(x => x.ContainsKey("cy")).Returns(true);
            _mockQueryCollection.Setup(x => x.ContainsKey("en")).Returns(false);

            var savedLanguage = "en-GB";
            var sessionValue = System.Text.Encoding.UTF8.GetBytes(savedLanguage);
            _mockSession.Setup(x => x.TryGetValue("Language", out sessionValue)).Returns(true);

            _mockSession.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<byte[]>()));

            var actionExecutingContext = CreateActionExecutingContext();

            // Act
            _controller.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Equal("cy", Thread.CurrentThread.CurrentCulture.Name);
            Assert.Equal("cy", Thread.CurrentThread.CurrentUICulture.Name);
        }

        [Fact]
        public void OnActionExecuting_EnParameterTakesPrecedenceOverSavedLanguage()
        {
            // Arrange
            _mockQueryCollection.Setup(x => x.ContainsKey("en")).Returns(true);
            _mockQueryCollection.Setup(x => x.ContainsKey("cy")).Returns(false);

            var savedLanguage = "cy";
            var sessionValue = System.Text.Encoding.UTF8.GetBytes(savedLanguage);
            _mockSession.Setup(x => x.TryGetValue("Language", out sessionValue)).Returns(true);

            _mockSession.Setup(x => x.Set(It.IsAny<string>(), It.IsAny<byte[]>()));

            var actionExecutingContext = CreateActionExecutingContext();

            // Act
            _controller.OnActionExecuting(actionExecutingContext);

            // Assert
            Assert.Equal("en-GB", Thread.CurrentThread.CurrentCulture.Name);
            Assert.Equal("en-GB", Thread.CurrentThread.CurrentUICulture.Name);
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("http://example.com")]
        public void SetBackUrl_WithVariousValues_SetsViewDataCorrectly(string backUrl)
        {
            // Arrange
            var expectedValue = string.IsNullOrWhiteSpace(backUrl) ? null : backUrl;

            // Act
            _controller.TestSetBackUrl(backUrl);

            // Assert
            var actualValue = _controller.ViewData[WebAppConstants.ViewKeys.BackUrl] as string;
            Assert.Equal(expectedValue, actualValue);
        }

       

        [Fact]
        public void CurrentUserId_WithUnauthenticatedUser_ReturnsEmptyGuid()
        {
            // Arrange
            var identity = new ClaimsIdentity();
            var principal = new ClaimsPrincipal(identity);

            _mockHttpContext.Setup(x => x.User).Returns(principal);
            _mockHttpContext.Setup(x => x.User.Identity.IsAuthenticated).Returns(false);

            // Act
            var result = _controller.CurrentUserId();

            // Assert
            Assert.Equal(Guid.Empty, result);
        }

        [Fact]
        public void SaveMagicWordFormData_WithValidModel_SavesDataToTempData()
        {
            // Arrange
            var model = new MagicWordViewModel { HasUserPassedPasswordCheck = true };

            // Act
            _controller.TestSaveMagicWordFormData(model);

            // Assert
            var savedModel = _tempData.GetHasUserUsedMagicWord();
            Assert.NotNull(savedModel);
            Assert.True(savedModel.HasUserPassedPasswordCheck);
        }

        [Fact]
        public void GetMagicWordFormData_WithExistingData_ReturnsData()
        {
            // Arrange
            var model = new MagicWordViewModel { HasUserPassedPasswordCheck = true };
            _tempData.SetHasUserUsedMagicWord(model);

            // Act
            var result = _controller.TestGetMagicWordFormData();

            // Assert
            Assert.NotNull(result);
            Assert.True(result.HasUserPassedPasswordCheck);
        }

        [Fact]
        public void GetMagicWordFormData_WithCreateIfNull_CreatesNewModel()
        {
            // Arrange & Act
            var result = _controller.TestGetMagicWordFormData(true);

            // Assert
            Assert.NotNull(result);
            Assert.False(result.HasUserPassedPasswordCheck);
        }

        [Fact]
        public void GetMagicWordFormData_WithoutCreateIfNull_ReturnsNull()
        {
            // Arrange & Act
            var result = _controller.TestGetMagicWordFormData(false);

            // Assert
            Assert.Null(result);
        }

       

        [Fact]
        public void OnActionExecuting_CallsBaseOnActionExecuting()
        {
            // Arrange
            var actionExecutingContext = CreateActionExecutingContext();
            _mockQueryCollection.Setup(x => x.ContainsKey("cy")).Returns(false);
            _mockQueryCollection.Setup(x => x.ContainsKey("en")).Returns(false);

            var sessionValue = (byte[])null;
            _mockSession.Setup(x => x.TryGetValue("Language", out sessionValue)).Returns(false);

            // Act
            _controller.OnActionExecuting(actionExecutingContext);

            // Assert - Test passes if no exception is thrown
            Assert.True(true);
        }

        private ActionExecutingContext CreateActionExecutingContext()
        {
            var actionContext = new ActionContext
            {
                HttpContext = _mockHttpContext.Object,
                RouteData = new Microsoft.AspNetCore.Routing.RouteData(),
                ActionDescriptor = new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
            };

            return new ActionExecutingContext(
                actionContext,
                new List<IFilterMetadata>(),
                new Dictionary<string, object>(),
                _controller);
        }
    }

    // Concrete implementation for testing
    public class ConcreteBaseController : BaseController
    {
        private readonly HttpContext _httpContext;

        public ConcreteBaseController(HttpContext httpContext)
        {
            _httpContext = httpContext;
        }

        public override HttpContext GetHttpContext() => _httpContext;

        // Test helper methods to expose protected methods
        public void TestSetBackUrl(string backUrl) => SetBackUrl(backUrl);
        public void TestSaveMagicWordFormData(MagicWordViewModel model) => SaveMagicWordFormData(model);
        public MagicWordViewModel TestGetMagicWordFormData(bool createIfNull = false) => GetMagicWordFormData(createIfNull);
        public void TestRemoveMagicWordFormData() => RemoveMagicWordFormData();
    }
}