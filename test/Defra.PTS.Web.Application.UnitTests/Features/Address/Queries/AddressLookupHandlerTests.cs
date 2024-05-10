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
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assert = NUnit.Framework.Assert;

namespace Defra.PTS.Web.Application.UnitTests.Features.Address.Queries
{
    public class AddressLookupHandlerTests
    {
        [Fact]
        public async Task Handle_AddressLookupHandler_ReturnsValidResponse()
        {
            // Arrange
            var adressLookupServiceMock = new Mock<IAddressLookupService>();
            var addressLoggerMock = new Mock<ILogger<AddressLookupHandler>>();

            var handler = new AddressLookupHandler(adressLookupServiceMock.Object, addressLoggerMock.Object);
            var request = new AddressLookupRequest("");

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Handle_AddressLookupHandler_ReturnsCustomException()
        {
            // Arrange
            var adressLookupServiceMock = new Mock<IAddressLookupService>();
            var addressLoggerMock = new Mock<ILogger<AddressLookupHandler>>();

            adressLookupServiceMock.Setup(x => x.FindAddressesByPostcode(It.IsAny<string>())).ThrowsAsync(new Exception());

            var handler = new AddressLookupHandler(adressLookupServiceMock.Object, addressLoggerMock.Object);
            var request = new AddressLookupRequest("");

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));
        }
    }
}
