using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Features.Lookups.Queries;
using Defra.PTS.Web.Application.Features.TravelDocument.Queries;
using Defra.PTS.Web.Application.Services.Interfaces;
using Defra.PTS.Web.Domain.Enums;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace Defra.PTS.Web.Application.UnitTests.Features.Lookups.Queries
{
    public class AddAddressHandlerTests
    {
        [Fact]
        public async Task Handle_GetBreedsQueryRequest_ReturnsValidResponse()
        {
            // Arrange
            var petServiceMock = new Mock<IPetService>();

            var loggerMock = new Mock<ILogger<GetBreedsQueryHandler>>();

            var handler = new GetBreedsQueryHandler(petServiceMock.Object, loggerMock.Object);
            var request = new GetBreedsQueryRequest(Domain.Enums.PetSpecies.Dog);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // Add more assertions here based on your expectations for the result
        }

        [Fact]
        public async Task Handle_GetBreedsQueryRequest_ReturnsCustomException()
        {
            // Arrange
            var petServiceMock = new Mock<IPetService>();

            var loggerMock = new Mock<ILogger<GetBreedsQueryHandler>>();

            petServiceMock.Setup(x => x.GetBreeds(It.IsAny<PetSpecies>())).ThrowsAsync(new Exception());

            var handler = new GetBreedsQueryHandler(petServiceMock.Object, loggerMock.Object);
            var request = new GetBreedsQueryRequest(Domain.Enums.PetSpecies.Dog);

            // Act
            Assert.Equal(Domain.Enums.PetSpecies.Dog, request.PetType);
            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));
        }
    }
}
