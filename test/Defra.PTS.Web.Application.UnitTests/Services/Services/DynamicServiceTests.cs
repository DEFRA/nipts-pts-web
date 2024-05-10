using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Microsoft.AspNetCore.Http;
using Defra.PTS.Web.Domain.Models;
using Microsoft.Extensions.Options;
using Defra.PTS.Web.Infrastructure.Models;
using System.Net;
using Assert = NUnit.Framework.Assert;
using Moq.Protected;
using Azure;
using static Microsoft.Azure.KeyVault.WebKey.JsonWebKeyVerifier;

namespace Defra.PTS.Web.Application.UnitTests.Services.Services
{
    [TestFixture]
    public class DynamicServiceTests
    {
        private IDynamicService _systemUnderTest;

        protected Mock<HttpMessageHandler> _mockHttpMessageHandler;
        private Mock<ILogger<DynamicService>> _mockLogger;
        private Mock<IOptions<AppSettings>> _options;
        private Mock<HttpClient> _httpClientMock;

        [SetUp]
        public void SetUp()
        {
            _mockHttpMessageHandler = new Mock<HttpMessageHandler>();
            _mockLogger = new Mock<ILogger<DynamicService>>();
            _options = new Mock<IOptions<AppSettings>>();
            _httpClientMock = new Mock<HttpClient>();
        }

        [Test]
        public void AddApplicationToQueueAsync_Exception()
        {
            // Arrange
            var application = new ApplicationSubmittedMessage();
            var expectedUrl = "http://yourapi.com/";

            _options.Setup(a => a.Value).Returns(new AppSettings() { DynamicServiceUrl = expectedUrl });
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ThrowsAsync(new Exception("Test exception"));

            var httpClientMock = new HttpClient(_mockHttpMessageHandler.Object);

            _systemUnderTest = new DynamicService(_options.Object, httpClientMock, _mockLogger.Object);

            // Act & Assert
            Assert.ThrowsAsync<Exception>(() => _systemUnderTest.AddApplicationToQueueAsync(application));
        }


        [Test]
        public async Task AddApplicationToQueueAsync_Success()
        {
            // Arrange
            var application = new ApplicationSubmittedMessage();
            var expectedUrl = "http://yourapi.com/";
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            _options.Setup(a => a.Value).Returns(new AppSettings() { DynamicServiceUrl = expectedUrl });
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);
            var httpClientMock = new HttpClient(_mockHttpMessageHandler.Object);
            _systemUnderTest = new DynamicService(_options.Object, httpClientMock, _mockLogger.Object);

            // Act
            await _systemUnderTest.AddApplicationToQueueAsync(application);

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                   "SendAsync",
                   Times.Once(),
                   ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                   ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task AddAddressAsync_Success()
        {
            // Arrange
            var user = new User { FirstName =  "Test"};
            var expectedUrl = "http://yourapi.com/";
            var response = new HttpResponseMessage(HttpStatusCode.OK);

            _options.Setup(a => a.Value).Returns(new AppSettings() { DynamicServiceUrl = expectedUrl });
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);
            var httpClientMock = new HttpClient(_mockHttpMessageHandler.Object);
            _systemUnderTest = new DynamicService(_options.Object, httpClientMock, _mockLogger.Object);

            // Act
            await _systemUnderTest.AddAddressAsync(user);

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                   "SendAsync",
                   Times.Once(),
                   ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                   ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task AddAddressAsync_Failure()
        {
            // Arrange
            var user = new User { FirstName = "Test" };
            var expectedUrl = "http://yourapi.com/";
            var response = new HttpResponseMessage(HttpStatusCode.Forbidden);

            _options.Setup(a => a.Value).Returns(new AppSettings() { DynamicServiceUrl = expectedUrl });
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);
            var httpClientMock = new HttpClient(_mockHttpMessageHandler.Object);
            _systemUnderTest = new DynamicService(_options.Object, httpClientMock, _mockLogger.Object);

            // Act
            await _systemUnderTest.AddAddressAsync(user);

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                   "SendAsync",
                   Times.Once(),
                   ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                   ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public async Task AddApplicationToQueueAsync_Failure()
        {
            // Arrange
            var application = new ApplicationSubmittedMessage();
            var expectedUrl = "http://yourapi.com/";
            var response = new HttpResponseMessage(HttpStatusCode.Forbidden);

            _options.Setup(a => a.Value).Returns(new AppSettings() { DynamicServiceUrl = expectedUrl });
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>()).ReturnsAsync(response);
            var httpClientMock = new HttpClient(_mockHttpMessageHandler.Object);
            _systemUnderTest = new DynamicService(_options.Object, httpClientMock, _mockLogger.Object);

            // Act
            await _systemUnderTest.AddApplicationToQueueAsync(application);

            // Assert
            _mockHttpMessageHandler.Protected().Verify(
                   "SendAsync",
                   Times.Once(),
                   ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Post),
                   ItExpr.IsAny<CancellationToken>());
        }

        [Test]
        public void AddApplicationToQueueAsync_ThrowsException()
        {
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception("Unable to fetch details"));

            // Arrange
            var application = new ApplicationSubmittedMessage();
            var expectedUrl = "http://yourapi.com/";
            var response = new HttpResponseMessage(HttpStatusCode.Forbidden);

            _options.Setup(a => a.Value).Returns(new AppSettings() { DynamicServiceUrl = expectedUrl });
            var httpClientMock = new HttpClient(_mockHttpMessageHandler.Object);
            _systemUnderTest = new DynamicService(_options.Object, httpClientMock, _mockLogger.Object);

            // Act
            Assert.ThrowsAsync<Exception>(async () => await _systemUnderTest.AddApplicationToQueueAsync(application));
        }

    }
}
