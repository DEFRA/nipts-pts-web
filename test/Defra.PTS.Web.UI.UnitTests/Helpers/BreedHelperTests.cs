using Defra.PTS.Web.Application.DTOs.Features;
using Defra.PTS.Web.Application.DTOs.Services;
using Defra.PTS.Web.Application.Features.Lookups.Queries;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.UI.Controllers;
using Defra.PTS.Web.UI.Helpers;
using FluentAssertions.Execution;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.WindowsAzure.Storage.Blob.Protocol;
using Moq;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        [Test]
        public async Task GetBreedListTests()
        {
            // Arrange
            Thread.CurrentThread.CurrentCulture = new CultureInfo("cy");

            var species = PetSpecies.Dog;
            _mockMediator.Setup(x => x.Send(It.IsAny<GetBreedsQueryRequest>(), CancellationToken.None))
                .ReturnsAsync(new Application.DTOs.Features.GetBreedsQueryResponse
                {
                    Breeds = new List<BreedDto>()
                    {
                        new BreedDto()
                        {
                            BreedId = 1,  BreedName = "Afghan Hound"
                        }

                    }           
                });

            // Act
            var result = await _breedHelper.GetBreedList(species);

            // Assert

        }


    }
}
