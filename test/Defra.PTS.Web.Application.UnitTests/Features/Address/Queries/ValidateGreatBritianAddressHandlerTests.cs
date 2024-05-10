using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Features.Address.Queries;
using Defra.PTS.Web.Application.Features.DynamicsCrm.Commands;
using Defra.PTS.Web.Application.Features.Lookups.Queries;
using Defra.PTS.Web.Application.Features.TravelDocument.Queries;
using Defra.PTS.Web.Application.Services;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
using Defra.PTS.Web.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Features.Address.Queries
{
    public class ValidateGreatBritianAddressHandlerTests
    {
        [Theory]
        [InlineData("")]
        [InlineData("BT93 8AD")]
        public async Task Handle_ValidateGreatBritianAddressHandler_NonGb_ReturnsValidResponse(string postcode)
        {
            // Arrange
            var adressLookupServiceMock = new Mock<IAddressLookupService>();
            var addressLoggerMock = new Mock<ILogger<ValidateGreatBritianAddressHandler>>();

            var handler = new ValidateGreatBritianAddressHandler(adressLookupServiceMock.Object, addressLoggerMock.Object);
            var request = new ValidateGreatBritianAddressRequest(postcode);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData("SW1A 2AB")]
        public async Task Handle_ValidateGreatBritianAddressHandler_Gb_ReturnsValidResponse(string postcode)
        {
            // Arrange
            var adressLookupServiceMock = new Mock<IAddressLookupService>();
            var addressLoggerMock = new Mock<ILogger<ValidateGreatBritianAddressHandler>>();
            List<Domain.Models.Address> addresses = new List<Domain.Models.Address>
            {
                new Domain.Models.Address()
            };

            adressLookupServiceMock.Setup(a => a.FindAddressesByPostcode(It.IsAny<string>())).Returns(Task.FromResult(addresses));

            var handler = new ValidateGreatBritianAddressHandler(adressLookupServiceMock.Object, addressLoggerMock.Object);
            var request = new ValidateGreatBritianAddressRequest(postcode);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.True(result);
        }

        [Fact]
        public async Task Handle_ValidateGreatBritianAddressHandler_HitsException_ReturnsFalse()
        {
            // Arrange
            var adressLookupServiceMock = new Mock<IAddressLookupService>();
            var addressLoggerMock = new Mock<ILogger<ValidateGreatBritianAddressHandler>>();
            List<Domain.Models.Address> addresses = new List<Domain.Models.Address>
            {
                new Domain.Models.Address()
            };

            adressLookupServiceMock.Setup(a => a.FindAddressesByPostcode(It.IsAny<string>())).ThrowsAsync(new Exception());

            var handler = new ValidateGreatBritianAddressHandler(adressLookupServiceMock.Object, addressLoggerMock.Object);
            var request = new ValidateGreatBritianAddressRequest("SW1A 2AB");

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.False(result);
        }
    }
}
