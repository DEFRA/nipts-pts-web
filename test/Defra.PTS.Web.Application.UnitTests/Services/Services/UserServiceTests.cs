using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Services;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using NUnit.Framework;
using System.Net;
using System.Text;
using static System.Net.Mime.MediaTypeNames;
using Assert = NUnit.Framework.Assert;

namespace Defra.PTS.Web.Application.UnitTests.Services.Services
{
    [TestFixture]
    public class UserServiceTests
    {
        private IUserService _sut;
        protected Mock<HttpMessageHandler> _mockHttpMessageHandler = new();
        private readonly Mock<ILogger<UserService>> _mockLogger = new();

        [Test]
        public async Task AddUser_Return_Success()
        {
            var user = new User 
            { 
                FirstName = "Bob",
                LastName = "Test",
                EmailAddress = "test@email.com",
                Role = "admin",
                ContactId = Guid.NewGuid().ToString(),
                UniqueReference = "unique"                
            };
            
            var expectedUserId = Guid.NewGuid();

            var jsonString = JsonConvert.SerializeObject(expectedUserId);
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

            _sut = new UserService(_mockLogger.Object, httpClient);

            var actualResult = await _sut.AddUserAsync(user);

            Assert.AreEqual(expectedUserId, actualResult);
        }

        [Test]
        public async Task AddAddress_Return_Success()
        {
            var travelDocumentViewModel = new TravelDocumentViewModel
            {
                  PetKeeperAddressManual = new PetKeeperAddressManualViewModel
                  {
                       AddressLineOne = "test lane"
                  }
            };

            var expectedUserId = Guid.NewGuid();

            var jsonString = JsonConvert.SerializeObject(expectedUserId);
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

            _sut = new UserService(_mockLogger.Object, httpClient);

            var actualResult = await _sut.AddAddressAsync(Domain.Enums.AddressType.Owner, travelDocumentViewModel);

            Assert.AreEqual(expectedUserId, actualResult);
        }

        [Test]
        public void AddAddress_Return_Failure()
        {
            var travelDocumentViewModel = new TravelDocumentViewModel
            {
                PetKeeperAddressManual = new PetKeeperAddressManualViewModel
                {
                    AddressLineOne = "test lane"
                }
            };

            var expectedUserId = Guid.NewGuid();

            var jsonString = JsonConvert.SerializeObject(expectedUserId);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = httpContent
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new UserService(_mockLogger.Object, httpClient);

            Assert.ThrowsAsync<HttpRequestException>(async () => await _sut.AddAddressAsync(Domain.Enums.AddressType.Owner, travelDocumentViewModel));
        }

        [Test]
        public void AddUser_BadRequest_ThrowsException()
        {
            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                FirstName = "Bob",
                LastName = "Test",
                EmailAddress = "test@email.com",
                Role = "admin",
                ContactId = Guid.NewGuid().ToString(),
                UniqueReference = "unique"
            };

            var expectedUserId = Guid.NewGuid();

            var jsonString = JsonConvert.SerializeObject(expectedUserId);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = httpContent
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new UserService(_mockLogger.Object, httpClient);

            Assert.ThrowsAsync<HttpRequestException>(async () => await _sut.AddUserAsync(user));
        }

        [Test]
        public void AddUser_ThrowsException()
        {
            var user = new User
            {
                UserId = Guid.NewGuid().ToString(),
                FirstName = "Bob",
                LastName = "Test",
                EmailAddress = "test@email.com",
                Role = "admin",
                ContactId = Guid.NewGuid().ToString(),
                UniqueReference = "unique"
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error"));

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new UserService(_mockLogger.Object, httpClient);

            Assert.ThrowsAsync<HttpRequestException>(async () => await _sut.AddUserAsync(user));
        }

        [Test]
        public async Task AddOwner_Return_Success()
        {
            var user = new User
            {
                FirstName = "Bob",
                LastName = "Test",
                EmailAddress = "test@email.com",
                Role = "admin",
                ContactId = Guid.NewGuid().ToString(),
                UniqueReference = "unique"
            };

            var travelDocument = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    AddressLineOne = "Line 1",
                    Postcode = "sw1 4tg",
                    TownOrCity = "London",
                    Phone = "119344234",
                    Email = "test@test.com",
                    Name = "Test", 
                    IsCompleted = true,
                    UserDetailsAreCorrect = Domain.Enums.YesNoOptions.Yes
                }
            };  

            travelDocument.PetKeeperUserDetails.TrimUnwantedData();

            var expectedOwnerId = Guid.NewGuid();

            var jsonString = JsonConvert.SerializeObject(expectedOwnerId);
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

            _sut = new UserService(_mockLogger.Object, httpClient);

            var actualResult = await _sut.AddOwnerAsync(user, travelDocument);

            Assert.AreEqual(expectedOwnerId, actualResult);
        }

        [Test]
        public void AddOwner_BadRequest_ThrowsException()
        {
            var user = new User
            {
                FirstName = "Bob",
                LastName = "Test",
                EmailAddress = "test@email.com",
                Role = "admin",
                ContactId = Guid.NewGuid().ToString(),
                UniqueReference = "unique"
            };

            var travelDocument = new TravelDocumentViewModel
            {
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    AddressLineOne = "Line 1",
                    Postcode = "sw1 4tg",
                    TownOrCity = "London",
                    Phone = "119344234",
                    Email = "test@test.com",
                    Name = "Test",
                    IsCompleted = true,
                    UserDetailsAreCorrect = Domain.Enums.YesNoOptions.Yes
                }
            };

            travelDocument.PetKeeperUserDetails.TrimUnwantedData();

            var expectedOwnerId = Guid.NewGuid();

            var jsonString = JsonConvert.SerializeObject(expectedOwnerId);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = httpContent
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new UserService(_mockLogger.Object, httpClient);

            Assert.ThrowsAsync<HttpRequestException>(async () => await _sut.AddOwnerAsync(user, travelDocument));
        }

        [Test]
        public void AddOwner_ThrowsException()
        {
            var user = new User
            {
                FirstName = "Bob",
                LastName = "Test",
                EmailAddress = "test@email.com",
                Role = "admin",
                ContactId = Guid.NewGuid().ToString(),
                UniqueReference = "unique"
            };

            var travelDocument = new TravelDocumentViewModel
            {
                IsApplicationInProgress = true,
                PetKeeperUserDetails = new PetKeeperUserDetailsViewModel
                {
                    AddressLineOne = "Line 1",
                    Postcode = "sw1 4tg",
                    TownOrCity = "London",
                    Phone = "119344234", 
                }, 
                IsSubmitted = true
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error"));

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new UserService(_mockLogger.Object, httpClient);

            Assert.ThrowsAsync<HttpRequestException>(async () => await _sut.AddOwnerAsync(user, travelDocument));
        }

        [Test]
        public async Task UpdateUser_Return_Success()
        {
            var expectedOwnerId = Guid.NewGuid();

            var jsonString = JsonConvert.SerializeObject(expectedOwnerId);
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

            _sut = new UserService(_mockLogger.Object, httpClient);

            var actualResult = await _sut.UpdateUserAsync("test@email.com");

            Assert.AreEqual(expectedOwnerId, actualResult);
        }

        [Test]
        public void UpdateUser_BadRequest_ThrowsException()
        {
            var expectedOwnerId = Guid.NewGuid();

            var jsonString = JsonConvert.SerializeObject(expectedOwnerId);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = httpContent
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new UserService(_mockLogger.Object, httpClient);

            Assert.ThrowsAsync<HttpRequestException>(async () => await _sut.UpdateUserAsync("test@email.com"));
        }

        [Test]
        public void UpdateUser_ThrowsException()
        {
            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error"));

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new UserService(_mockLogger.Object, httpClient);

            Assert.ThrowsAsync<HttpRequestException>(async () => await _sut.UpdateUserAsync("test@email.com"));
        }

        [Test]
        public async Task UpdateAddress_Return_Success()
        {
            var expectedOwnerId = Guid.NewGuid();

            var jsonString = JsonConvert.SerializeObject(expectedOwnerId);
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

            _sut = new UserService(_mockLogger.Object, httpClient);

            var actualResult = await _sut.UpdateUserAddressAsync("test@email.com", expectedOwnerId);

            Assert.AreEqual(expectedOwnerId, actualResult);
        }

        [Test]
        public void UpdateAddress_BadRequest_ThrowsException()
        {
            var expectedOwnerId = Guid.NewGuid();

            var jsonString = JsonConvert.SerializeObject(expectedOwnerId);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadRequest,
                Content = httpContent
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(expectedResponse);

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new UserService(_mockLogger.Object, httpClient);

            Assert.ThrowsAsync<HttpRequestException>(async () => await _sut.UpdateUserAddressAsync("test@email.com", expectedOwnerId));
        }

        [Test]
        public void UpdateAddress_ThrowsException()
        {
            var expectedOwnerId = Guid.NewGuid();

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error"));

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new UserService(_mockLogger.Object, httpClient);

            Assert.ThrowsAsync<HttpRequestException>(async () => await _sut.UpdateUserAddressAsync("test@email.com", expectedOwnerId));
        }

        [Test]
        public async Task GetUserDetail_Return_Success()
        {
            var expectedUserId = Guid.NewGuid();

            var userDetail = new UserDetailDto
            {
                FullName = "Test"
            };


            var jsonString = JsonConvert.SerializeObject(userDetail);
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

            _sut = new UserService(_mockLogger.Object, httpClient);

            var actualResult = await _sut.GetUserDetail(expectedUserId);

            Assert.AreEqual(userDetail.FullName, actualResult.FullName);
        }

        [Test]
        public void GetUserDetail_Throws_Exception()
        {
            var expectedUserId = Guid.NewGuid();

            var userDetail = new UserDetailDto
            {
                FullName = "Test"
            };


            var jsonString = JsonConvert.SerializeObject(userDetail);
            var httpContent = new StringContent(jsonString, Encoding.UTF8, "application/json");

            var expectedResponse = new HttpResponseMessage
            {
                StatusCode = HttpStatusCode.BadGateway,
                Content = httpContent
            };

            _mockHttpMessageHandler.Protected().Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ThrowsAsync(new HttpRequestException("Error"));

            var httpClient = new HttpClient(_mockHttpMessageHandler.Object)
            {
                BaseAddress = new Uri("https://localhost/")
            };

            _sut = new UserService(_mockLogger.Object, httpClient);

            Assert.ThrowsAsync<HttpRequestException>(async () => await _sut.GetUserDetail(expectedUserId));
        }

    }
}
