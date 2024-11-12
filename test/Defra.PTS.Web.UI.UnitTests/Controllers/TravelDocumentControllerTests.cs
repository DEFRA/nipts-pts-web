using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Features.DynamicsCrm.Commands;
using Defra.PTS.Web.Application.Features.TravelDocument.Queries;
using Defra.PTS.Web.Application.Features.Users.Commands;
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
using Microsoft.Azure.Amqp.Transaction;
using Microsoft.Azure.Management.ContainerRegistry.Fluent.Models;
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
    public class TravelDocumentControllerTests
    {
        private readonly Mock<IValidationService> _mockValidationService = new();
        private readonly Mock<IMediator> _mockMediator = new();
        private readonly Mock<ILogger<TravelDocumentController>> _mockLogger = new();
        private IOptions<PtsSettings> _optionsPtsSettings;
        private TravelDocumentController _travelDocumentController;

        private readonly IStringLocalizer<ISharedResource> _localizer;
        private readonly Mock<ISelectListLocaliser> _breedHelper = new();

        public TravelDocumentControllerTests()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<ISharedResource>(factory);

        }

        [SetUp]
        public void Setup()
        {
            var ptsSettings = new PtsSettings
            {
                MagicWordEnabled = true,
            };
            _optionsPtsSettings = Options.Create(ptsSettings);
            _travelDocumentController = new TravelDocumentController(_mockValidationService.Object, _mockMediator.Object, _mockLogger.Object, _optionsPtsSettings, _breedHelper.Object, _localizer);
        }


        [Test]
        public void If_MagicWordEnabled_True_RedirectTo_Index()
        {
            // Arrange

            var mockHttpContext = new Mock<Microsoft.AspNetCore.Http.IHttpContextAccessor>();
            var mockRequest = new Mock<Microsoft.AspNetCore.Http.HttpRequest>();
            var mockResponse = new Mock<Microsoft.AspNetCore.Http.HttpResponse>();
            var mockCookies = new Mock<Microsoft.AspNetCore.Http.IRequestCookieCollection>();

            mockHttpContext.Setup(x => x.HttpContext.Request).Returns(mockRequest.Object);
            mockHttpContext.Setup(x => x.HttpContext.Response).Returns(mockResponse.Object);
            mockRequest.Setup(x => x.Cookies).Returns(mockCookies.Object);
            mockCookies.Setup(x => x.TryGetValue("ManagementLinkClicked", out It.Ref<string>.IsAny)).Returns(true);
            mockCookies.Setup(x => x["ManagementLinkClicked"]).Returns("true");

            _travelDocumentController.ControllerContext.HttpContext = mockHttpContext.Object.HttpContext;

            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<Microsoft.AspNetCore.Http.HttpContext>(), Mock.Of<ITempDataProvider>());
            var magicWordViewModel = new MagicWordViewModel { HasUserPassedPasswordCheck = false };
            tempData.SetHasUserUsedMagicWord(magicWordViewModel);
            _travelDocumentController.TempData = tempData;

            // Act
            var result = _travelDocumentController.Index().Result as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.Index), result.ActionName);
        }


        [Test]
        public void If_HasUserPassedPasswordCheck_True_Returns_View()
        {
            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<Microsoft.AspNetCore.Http.HttpContext>(), Mock.Of<ITempDataProvider>());
            var magicWordViewModel = new MagicWordViewModel { HasUserPassedPasswordCheck = true };
            tempData.SetHasUserUsedMagicWord(magicWordViewModel);
            _travelDocumentController.TempData = tempData;
            MockHttpContext();
            _mockMediator.Setup(x => x.Send(It.IsAny<AddUserRequest>(), CancellationToken.None))
               .ReturnsAsync(new AddUserResponse
               {
                   IsSuccess = true
               });
            _mockMediator.Setup(x => x.Send(It.IsAny<GetApplicationsQueryRequest>(), CancellationToken.None))
             .ReturnsAsync(new GetApplicationsQueryResponse
             {
                 Applications = [new ApplicationSummaryDto { ApplicationId = Guid.NewGuid() }]
             });
            // Act
            var result = _travelDocumentController.Index().Result as ViewResult;

            // Assert
            Assert.IsNotNull(result);
        }

        [Test]
        public void ApplicationDetailRecord_WithValidModel_RedirectsTo_ApplicationCertificate()
        {
            //Arrange
            MockHttpContext();

            // Act
            var result = _travelDocumentController.ApplicationDetailRecord("1", AppConstants.ApplicationStatus.APPROVED) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.ApplicationCertificate), result.ActionName);
        }
        [Test]
        public void ApplicationDetailRecord_WithValidModel_RedirectsTo_ApplicationDetails()
        {
            //Arrange
            MockHttpContext();

            // Act
            var result = _travelDocumentController.ApplicationDetailRecord("1", AppConstants.ApplicationStatus.UNSUCCESSFUL) as RedirectToActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(nameof(TravelDocumentController.ApplicationDetails), result.ActionName);
        }

        [Test]
        public void GetHttpContext_ShouldReturnHttpContext()
        {
            // Arrange

            var expectedHttpContext = new DefaultHttpContext();
            _travelDocumentController.ControllerContext.HttpContext = expectedHttpContext;

            // Act
            var result = _travelDocumentController.GetHttpContext();

            // Assert
            Assert.AreEqual(expectedHttpContext, result);
        }

        [Test]
        public void CurrentUserContactId_WhenUserIsAuthenticated_ShouldReturnContactId()
        {
            // Arrange
            var expectedContactId = new Guid("00000000-0000-0000-0000-000000000000");
            var identity = new ClaimsIdentity(
            [
                new Claim("contactId", expectedContactId.ToString())
            ]);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _travelDocumentController.ControllerContext = new ControllerContext
            {
                HttpContext = new DefaultHttpContext
                {
                    User = claimsPrincipal
                }
            };

            // Act
            var result = _travelDocumentController.CurrentUserContactId();

            // Assert
            Assert.AreEqual(expectedContactId, result);
        }

        [Test]
        public void CurrentUserContactId_WhenUserIsNotAuthenticated_ShouldReturnEmptyGuid()
        {
            // Arrange
            _travelDocumentController.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = _travelDocumentController.CurrentUserContactId();

            // Assert
            Assert.AreEqual(Guid.Empty, result);
        }

        [Test]
        public void GetCurrentUserInfo_WhenUserIsAuthenticated_ShouldReturnUserWithClaims()
        {
            // Arrange
            var expectedUser = new User
            {
                ContactId = "12345678",
                UniqueReference = "ABC123",
                FirstName = "John",
                LastName = "Doe",
                EmailAddress = "john.doe@example.com",
                Role = "Admin"
            };
            var identity = new ClaimsIdentity(
            [
                new Claim("contactId", expectedUser.ContactId),
                new Claim("uniqueReference", expectedUser.UniqueReference),
                new Claim("firstName", expectedUser.FirstName),
                new Claim("lastName", expectedUser.LastName),
                new Claim("http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress", expectedUser.EmailAddress),
                new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", expectedUser.Role)
            ]);
            var claimsPrincipal = new ClaimsPrincipal(identity);
            _travelDocumentController.ControllerContext.HttpContext = new DefaultHttpContext
            {
                User = claimsPrincipal
            };

            // Act
            var result = _travelDocumentController.GetCurrentUserInfo();

            // Assert
            Assert.AreEqual(expectedUser.ContactId, result.ContactId);
            Assert.AreEqual(expectedUser.UniqueReference, result.UniqueReference);
            Assert.AreEqual(expectedUser.FirstName, result.FirstName);
            Assert.AreEqual(expectedUser.LastName, result.LastName);
            Assert.AreEqual(expectedUser.EmailAddress, result.EmailAddress);
            Assert.AreEqual(expectedUser.Role, result.Role);
        }

        [Test]
        public void GetCurrentUserInfo_WhenUserIsNotAuthenticated_ShouldReturnEmptyUser()
        {
            // Arrange
            _travelDocumentController.ControllerContext.HttpContext = new DefaultHttpContext();

            // Act
            var result = _travelDocumentController.GetCurrentUserInfo();

            // Assert
            Assert.IsNotNull(result);
        }

        private void MockHttpContext()
        {
            // Arrange
            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("example.com");
            var session = new MockHttpSession();
            httpContext.Session = session;

            var identityMock = new Mock<ClaimsIdentity>();
            identityMock.SetupGet(i => i.IsAuthenticated).Returns(true);
            var identities = new List<ClaimsIdentity>();
            // Create claims
            var claims = new List<Claim>
            {
                new("contactId", "123"),
                new("uniqueReference", "abc"),
                new("firstName", "John"),
                new("lastName", "Doe"),
                new(ClaimTypes.Email, "john.doe@example.com"),
                new(ClaimTypes.Role, "Admin")
            };

            identities.Add(identityMock.Object);
            var user = new ClaimsPrincipal(new ClaimsIdentity(claims));
            httpContext.User = user;
            _travelDocumentController.ControllerContext.HttpContext = httpContext;
        }
    }

    public class MockHttpSession : ISession
    {
        // Implement the methods and properties of ISession interface
        // For example, you can use a Dictionary to simulate session data

        // Here's a simple implementation for demonstration purposes
        private readonly Dictionary<string, byte[]> _sessionData = [];

        public byte[] this[string key]
        {
            get => _sessionData.TryGetValue(key, out var value) ? value : null;
            set => _sessionData[key] = value;
        }

        public IEnumerable<string> Keys => _sessionData.Keys;

        public string Id { get; set; }

        public bool IsAvailable => throw new NotImplementedException();

        public bool TryGetValue(string key, out byte[] value) => _sessionData.TryGetValue(key, out value);

        public void Set(string key, byte[] value) => _sessionData[key] = value;

        public void Remove(string key) => _sessionData.Remove(key);

        public Task LoadAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task CommitAsync(CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        // Implement other methods and properties as needed
    }

}
