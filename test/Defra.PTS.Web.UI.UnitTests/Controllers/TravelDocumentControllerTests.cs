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
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.Extensions.Logging;
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
        private Mock<ControllerContext> _mockControllerContext;
        private Mock<TravelDocumentViewModel> _travelDocumentViewModel;


        [SetUp]
        public void Setup()
        {
            var ptsSettings = new PtsSettings
            {
               MagicWordEnabled = true,
            };
            _mockControllerContext = new Mock<ControllerContext>();
            _optionsPtsSettings = Options.Create(ptsSettings);
            _travelDocumentController = new TravelDocumentController(_mockValidationService.Object, _mockMediator.Object, _mockLogger.Object, _optionsPtsSettings);
            
            
            _travelDocumentViewModel = new Mock<TravelDocumentViewModel>();
        }


        [Test]
        public void If_MagicWordEnabled_True_RedirectTo_Index()
        {
            // Arrange
            var tempData = new TempDataDictionary(Mock.Of<Microsoft.AspNetCore.Http.HttpContext>(), Mock.Of<ITempDataProvider>());
            var magicWordViewModel = new MagicWordViewModel { HasUserPassedPasswordCheck = false};
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
                 Applications = new List<ApplicationSummaryDto> { new ApplicationSummaryDto { ApplicationId = Guid.NewGuid() } }
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
                new Claim("contactId", "123"),
                new Claim("uniqueReference", "abc"),
                new Claim("firstName", "John"),
                new Claim("lastName", "Doe"),
                new Claim(ClaimTypes.Email, "john.doe@example.com"),
                new Claim(ClaimTypes.Role, "Admin")
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
        private readonly Dictionary<string, byte[]> _sessionData = new Dictionary<string, byte[]>();

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
