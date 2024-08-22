using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Features.Lookups.Queries;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.UI.Helpers;
using FluentAssertions;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System.Globalization;
using Xunit;

namespace Defra.PTS.Web.UI.UnitTests.Helpers
{
    public class BreedHelperTests
    {
        private readonly IStringLocalizer<SharedResource> _localizer;
        private readonly Mock<IMediator> _mockMediator = new();

        BreedHelper _breedHelper;
        public BreedHelperTests()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<SharedResource>(factory);
            _breedHelper = new BreedHelper(_mockMediator.Object, _localizer);

        }

        [Fact]
        public async Task GetBreedListTests()
        {
            // Arrange
            var species = PetSpecies.Dog;
            _mockMediator.Setup(x => x.Send(It.IsAny<GetBreedsQueryRequest>(), CancellationToken.None))
                .ReturnsAsync(new Application.DTOs.Features.GetBreedsQueryResponse
                {
                    Breeds = new List<BreedDto>()
                    {
                    new BreedDto() { BreedId = 1, BreedName = "Chihuahua" },
                    new BreedDto() { BreedId = 2, BreedName = "Pomeranian" },
                    new BreedDto() { BreedId = 3, BreedName = "Toy Poodle" },
                    new BreedDto() { BreedId = 4, BreedName = "Italian Greyhound" },
                    new BreedDto() { BreedId = 5, BreedName = "Whippet" },
                    }
                });

            var breed = new List<BreedDto>()
            {
                new BreedDto() { BreedId = 1, BreedName = "Chihuahua" },
                new BreedDto() { BreedId = 2, BreedName = "Pomeranian" },
                new BreedDto() { BreedId = 3, BreedName = "Toy Poodle" },
                new BreedDto() { BreedId = 4, BreedName = "Italian Greyhound" },
                new BreedDto() { BreedId = 5, BreedName = "Whippet" },

            };

            // Act
            var result = await _breedHelper.GetBreedList(species);

            // Assert

            result.Should().Equal(breed, (b1, b2) => b1.BreedId == b2.BreedId);
            result.Should().Equal(breed, (b1, b2) => b1.BreedName == b2.BreedName);

        }

    }
}
