using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Application.Services;
using Microsoft.Extensions.Logging;
using Moq;
using NUnit.Framework;
using System.Text;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using Defra.PTS.Web.Application.DTOs.Services;
using Assert = NUnit.Framework.Assert;
using AutoMapper;
using Defra.Trade.Address.V1.ApiClient.Model;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Azure.Management.Compute.Fluent.Models;
using Defra.PTS.Web.Application.Mapping;
using Microsoft.Extensions.DependencyInjection;
using Defra.PTS.Web.Application.Mapping.Converters;
using Defra.PTS.Web.QRCoder.Services.Interfaces;
using Defra.PTS.Web.QRCoder.Services;

namespace Defra.PTS.Web.Application.UnitTests.Services.Services
{
    [TestFixture]
    public class ApplicationServiceTests
    {
        private IApplicationService _sut;
        protected Mock<HttpMessageHandler> _mockHttpMessageHandler = new();
        private readonly Mock<ILogger<ApplicationService>> _mockLogger = new();
        private readonly Mock<IMapper> _mapper = new();

        [Test]
        public async Task CreateApplication_Return_Success()
        {
            var expectedResult = new ApplicationDto {  Id = Guid.NewGuid() };
            var jsonString = JsonConvert.SerializeObject(expectedResult);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.Created,
                Content = httpContent
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new ApplicationService(_mockLogger.Object, httpClient, _mapper.Object);

            var actualResult = await _sut.CreateApplication(new ApplicationDto());

            Assert.AreEqual(expectedResult.Id, actualResult.Id);
        }

        [Test]
        public void CreateApplication_ThrowsException()
        {
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception("Error"));

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new ApplicationService(_mockLogger.Object, httpClient, _mapper.Object);

            Assert.ThrowsAsync<Exception>(async () => await _sut.CreateApplication(new ApplicationDto()));
        }

        [Test]
        public async Task GetApplicationCertificate_Return_200()
        {
            IMapper mapper = GetMapper();

            var isueDate = DateTime.UtcNow;
            var guid = new Guid(Guid.NewGuid().ToString());


            var applicationCertificate = new VwApplication
            {
                ApplicationId = guid,
                DocumentReferenceNumber = "test",
                DocumentIssueDate = isueDate,
                PetGenderId = 1,
                PetSpeciesId = 1,
                PetBreedName = "Mixed", 
                PetBreedOther = "test", 
                PetColourName = "Other",
                PetColourOther = "test"
            };

            var jsonString = JsonConvert.SerializeObject(applicationCertificate);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = httpContent
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new ApplicationService(_mockLogger.Object, httpClient, mapper);

            var actualResult = await _sut.GetApplicationCertificate(Guid.NewGuid());

            Assert.AreEqual(applicationCertificate.DocumentReferenceNumber, actualResult.CertificateIssued.DocumentReferenceNumber);
        }

        [Test]
        public async Task GetApplicationDetails_Return_200()
        {
            IMapper mapper = GetMapper();

            var applicationDetails = new VwApplication
            {
                MicrochipNumber = "test"
            };

            var jsonString = JsonConvert.SerializeObject(applicationDetails);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = httpContent
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
            .ReturnsAsync(expectedResponse);

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new ApplicationService(_mockLogger.Object, httpClient, mapper);

            var actualResult = await _sut.GetApplicationDetails(Guid.NewGuid());

            Assert.AreEqual(applicationDetails.MicrochipNumber, actualResult.MicrochipInformation.MicrochipNumber);
        }

        [Test]
        public void GetApplicationDetails_ThrowsException()
        {
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception("Unable to fetch details"));

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new ApplicationService(_mockLogger.Object, httpClient, _mapper.Object);

            Assert.ThrowsAsync<Exception>(async () => await _sut.GetApplicationDetails(Guid.NewGuid()));
        }

        [Test]
        public void GetApplicationCertificates_ThrowsException()
        {
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception("Unable to fetch certificates"));

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new ApplicationService(_mockLogger.Object, httpClient, _mapper.Object);

            Assert.ThrowsAsync<Exception>(async () => await _sut.GetApplicationCertificate(Guid.NewGuid()));
        }

        [Test]
        public async Task GetApplications_Return_200()
        {
            var applications = new List<ApplicationSummaryDto> { new() { ApplicationId = Guid.NewGuid() } };
            var jsonString = JsonConvert.SerializeObject(applications);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.OK,
                Content = httpContent
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new ApplicationService(_mockLogger.Object, httpClient, _mapper.Object);

            var actualResult = await _sut.GetUserApplications(userId: Guid.NewGuid());

            Assert.AreEqual(applications[0].ApplicationId, actualResult[0].ApplicationId);
        }

        [Test]
        public void GetApplications_ThrowsException()
        {
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new Exception("Unable to fetch breeds"));

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new ApplicationService(_mockLogger.Object, httpClient, _mapper.Object);

            Assert.ThrowsAsync<Exception>(async () => await _sut.GetUserApplications(userId: Guid.NewGuid()));
        }

        private static IMapper GetMapper()
        {
            var services = new ServiceCollection();
            services.AddScoped<IQRCodeService, QRCodeService>();
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            var serviceProvider = services.BuildServiceProvider();
            return serviceProvider.GetService<IMapper>();
        }
    }
}
