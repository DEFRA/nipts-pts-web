﻿using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Features.DynamicsCrm.Commands;
using Defra.PTS.Web.Application.Features.Lookups.Queries;
using Defra.PTS.Web.Application.Features.TravelDocument.Queries;
using Defra.PTS.Web.Application.Services;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Models;
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

namespace Defra.PTS.Web.Application.UnitTests.Features.DynamicsCrm.Commands
{
    public class AddressLookupHandlerTests
    {
        [Fact]
        public async Task Handle_AddAddressRequest_ReturnsValidResponse()
        {
            // Arrange
            var addressLoggerMock = new Mock<ILogger<AddAddressHandler>>();
            var mockDynamicsService = new Mock<IDynamicService>();

            var user = new User();
            var handler = new AddAddressHandler(mockDynamicsService.Object, addressLoggerMock.Object);
            var request = new AddAddressRequest(user);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
        }

        [Fact]
        public void Handle_AddAddressRequest_ReturnsCustomException()
        {
            // Arrange
            var dynamicServiceMock = new Mock<IDynamicService>();

            var loggerMock = new Mock<ILogger<AddAddressHandler>>();

            var user = new User();

            dynamicServiceMock.Setup(x => x.AddAddressAsync(It.IsAny<User>())).ThrowsAsync(new Exception());

            var handler = new AddAddressHandler(dynamicServiceMock.Object, loggerMock.Object);
            var request = new AddAddressRequest(user);

            // Assert
            Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));
        }
    }
}
