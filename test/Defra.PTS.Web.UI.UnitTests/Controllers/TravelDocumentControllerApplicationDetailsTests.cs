﻿using Defra.PTS.Web.Application.Constants;
using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Extensions;
using Defra.PTS.Web.Application.Features.DynamicsCrm.Commands;
using Defra.PTS.Web.Application.Features.TravelDocument.Queries;
using Defra.PTS.Web.Application.Features.Users.Queries;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.CertificateGenerator.Models;
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
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using NUnit.Framework;
using PdfSharp.Drawing;
using PdfSharp.Pdf;
using System.Security.Claims;
using System.Text;
using System.IO;
using Assert = NUnit.Framework.Assert;
using Defra.PTS.Web.Application.Features.Certificates.Commands;
using Defra.PTS.Web.Application.Helpers;

namespace Defra.PTS.Web.UI.UnitTests.Controllers
{
    [TestFixture]
    public class TravelDocumentControllerApplicationDetailsTests
    {
        private readonly Mock<IValidationService> _mockValidationService = new();
        private readonly Mock<IMediator> _mockMediator = new();
        private readonly Mock<ILogger<TravelDocumentController>> _mockLogger = new();
        private readonly Mock<IOptions<PtsSettings>> _mockPtsSettings = new();
        private readonly Mock<ISelectListLocaliser> _mockBreedHelper = new();
        private Mock<TravelDocumentController> _travelDocumentController;
        private Mock<ControllerContext> _mockControllerContext;
        private Mock<TravelDocumentViewModel> _travelDocumentViewModel;
        private readonly IStringLocalizer<ISharedResource> _localizer;

        public TravelDocumentControllerApplicationDetailsTests()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<ISharedResource>(factory);
        }
        [SetUp]
        public void Setup()
        {
            _mockControllerContext = new Mock<ControllerContext>();
            _travelDocumentController = new Mock<TravelDocumentController>(_mockValidationService.Object, _mockMediator.Object, _mockLogger.Object, _mockPtsSettings.Object, _mockBreedHelper.Object, _localizer)
            {                
                CallBase = true,
                
            };

            var httpContext = new DefaultHttpContext();
            httpContext.Request.Scheme = "http";
            httpContext.Request.Host = new HostString("example.com");
            var session = new MockHttpSession();
            httpContext.Session = session;
            httpContext.Session.SetString("ApplicationId", Guid.NewGuid().ToString());

            _travelDocumentController.Object.ControllerContext.HttpContext = httpContext;

            _travelDocumentViewModel = new Mock<TravelDocumentViewModel>();
        }


        [Test]
        public void PetKeeperUserDetails_Returns_RedirectToAction_When_Application_NotInProgress()
        {
            // Arrange
            var applicationId = Guid.NewGuid();
            
            _mockMediator.Setup(x => x.Send(It.IsAny<GetApplicationDetailsQueryRequest>(), CancellationToken.None))
                 .ReturnsAsync(new GetApplicationDetailsQueryResponse
                 {
                     ApplicationDetails = new ApplicationDetailsDto()
                     {
                         ApplicationId = applicationId
                     }
                 });

            var result =  _travelDocumentController.Object.ApplicationDetails(applicationId).Result as ViewResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void ApplicationDetails_Returns_View()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();
            var applicationDetails = new ApplicationDetailsDto()
            {
                ApplicationId = applicationId,
                UserId = userId,
            };
            applicationDetails.Status = AppConstants.ApplicationStatus.AWAITINGVERIFICATION;


            _mockMediator.Setup(x => x.Send(It.IsAny<GetApplicationDetailsQueryRequest>(), CancellationToken.None))
                 .ReturnsAsync(new GetApplicationDetailsQueryResponse
                 {
                     ApplicationDetails = applicationDetails
                 });

            // different UserId
            _travelDocumentController.Setup(x => x.CurrentUserId()).Returns(userId);

            var result = _travelDocumentController.Object.ApplicationDetails(applicationId).Result as ViewResult;

            Assert.IsNotNull(result);
        }

        [Test]
        public void ApplicationDetails_Returns_Error()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var applicationId = Guid.NewGuid();
            var applicationDetails = new ApplicationDetailsDto()
            {
                ApplicationId = applicationId,
                UserId = userId,
            };
            applicationDetails.Status = AppConstants.ApplicationStatus.AWAITINGVERIFICATION;


            _mockMediator.Setup(x => x.Send(It.IsAny<GetApplicationDetailsQueryRequest>(), CancellationToken.None))
                 .ReturnsAsync(new GetApplicationDetailsQueryResponse
                 {
                     ApplicationDetails = applicationDetails
                 });

            // different UserId
            _travelDocumentController.Setup(x => x.CurrentUserId()).Returns(Guid.NewGuid());

            var result = _travelDocumentController.Object.ApplicationDetails(applicationId).Result as ViewResult;

            Assert.IsNull(result);
        }

        [Test]
        public async Task SetFileTitle_ShouldSetTitleForApplicationPdfAndReturnFile()
        {
            // Arrange
            var mockContent = CreateSamplePdfStream();
            var response = new CertificateResult(
                "SampleApplicationDetailsName",  // Name parameter
                mockContent,              // Stream parameter
                "application/pdf"         // MimeType parameter
            );

            var fileName = "test.pdf";
            var fileTitle = "Test Title";

            // Act
            var result = await _travelDocumentController.Object.SetFileTitle(response, fileName, fileTitle) as FileContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("application/pdf", result.ContentType);
            Assert.AreEqual(fileName, result.FileDownloadName);
        }

        [Test]
        public async Task DownloadApplicationDetailsPdf_ShouldReturnNotFoundWhenResponseIsNull()
        {
            // Arrange
            var id = Guid.NewGuid();
            var referenceNumber = "12345";
            _mockMediator
                .Setup(m => m.Send(It.IsAny<GenerateApplicationPdfRequest>(), default))
                .ReturnsAsync((CertificateResult)null);

            // Act
            var result = await _travelDocumentController.Object.DownloadApplicationDetailsPdf(id, referenceNumber);

            // Assert
            Assert.IsInstanceOf<NotFoundObjectResult>(result);
            var notFoundResult = result as NotFoundObjectResult;
            Assert.AreEqual("Unable to download the PDF", notFoundResult.Value);
        }

        [Test]
        public async Task DownloadApplicationDetailsPdf_ShouldCallSetFileTitleAndReturnFile()
        {
            // Arrange
            var id = Guid.NewGuid();
            var referenceNumber = "12345";
            var mockContent = CreateSamplePdfStream();

            var response = new CertificateResult(            
                "SampleApplicationDetailsName",  // Name parameter
                mockContent,              // Stream parameter
                "application/pdf"         // MimeType parameter
            );

            _mockMediator
                .Setup(m => m.Send(It.IsAny<GenerateApplicationPdfRequest>(), default))
                .ReturnsAsync(response);

            var fileName = ApplicationHelper.BuildPdfDownloadFilename(referenceNumber);
            var fileTitle = "Application number: " + referenceNumber + ".pdf";

            // Act
            var result = await _travelDocumentController.Object.DownloadApplicationDetailsPdf(id, referenceNumber) as FileContentResult;

            // Assert
            Assert.NotNull(result);
            Assert.AreEqual("application/pdf", result.ContentType);
            Assert.AreEqual(fileName, result.FileDownloadName);
        }

        public MemoryStream CreateSamplePdfStream()
        {
            // Create a new PDF document
            var document = new PdfDocument();
            document.Info.Title = "Sample PDF Document";

            // Add a page to the document
            var page = document.AddPage();

            // Create graphics for the page
            var graphics = XGraphics.FromPdfPage(page);

            // Define a font and draw some text
            var font = new XFont("Arial", 20, XFontStyleEx.Bold);
            graphics.DrawString("Hello, this is a test PDF!", font, XBrushes.Black,
                new XRect(0, 0, page.Width, page.Height), XStringFormats.Center);

            // Save the document into a MemoryStream
            var memoryStream = new MemoryStream();
            document.Save(memoryStream);

            // Reset the stream's position to the beginning
            memoryStream.Position = 0;

            return memoryStream;
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

}
