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
    public class GetColoursQueryHandlerTests
    {
        [Fact]
        public async Task Handle_GetColoursQueryRequest_ReturnsValidResponse()
        {
            // Arrange
            var petServiceMock = new Mock<IPetService>();

            var handler = new GetColoursQueryHandler(petServiceMock.Object);
            var request = new GetColoursQueryRequest(Domain.Enums.PetSpecies.Dog);

            // Act
            var result = await handler.Handle(request, CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            // Add more assertions here based on your expectations for the result
        }

        [Fact]
        public async Task Handle_GetColoursQueryRequest_ReturnsCustomException()
        {
            // Arrange
            var petServiceMock = new Mock<IPetService>();

            var loggerMock = new Mock<ILogger<GetColoursQueryHandler>>();

            petServiceMock.Setup(x => x.GetColours(It.IsAny<PetSpecies>())).ThrowsAsync(new Exception());

            var handler = new GetColoursQueryHandler(petServiceMock.Object);
            var request = new GetColoursQueryRequest(Domain.Enums.PetSpecies.Dog);

            // Act
            Assert.Equal(Domain.Enums.PetSpecies.Dog, request.PetType);
            await Assert.ThrowsAsync<Exception>(async () => await handler.Handle(request, CancellationToken.None));
        }
    }
}
