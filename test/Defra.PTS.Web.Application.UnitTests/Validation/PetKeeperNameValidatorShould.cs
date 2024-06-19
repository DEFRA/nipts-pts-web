using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetKeeperNameValidatorShould
    {
        private readonly IStringLocalizer<PetKeeperNameViewModel> _localizer;
        public PetKeeperNameValidatorShould()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<PetKeeperNameViewModel>(factory);
        }

        [Fact]
        public async Task NotHaveErrorName()
        {
            // Arrange
            var model = new PetKeeperNameViewModel() { Name = "Name" };
            var validator = new PetKeeperNameValidator(_localizer);

            // Act
            var result = await validator.TestValidateAsync(model);

            // Assert
            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [MemberData(nameof(PetKeeperNameTestData))]
        public async Task HaveErrorIfNameInvalid(string name)
        {
            // Arrange
            var model = new PetKeeperNameViewModel() { Name = name };
            var validator = new PetKeeperNameValidator(_localizer);

            // Act
            var result = await validator.TestValidateAsync(model);

            // Assert
            result.ShouldHaveValidationErrorFor(x => x.Name);
        }

        public static IEnumerable<object[]> PetKeeperNameTestData => new List<object[]>
        {
            new object[] { string.Empty },
            new object[] { new string('a', 301) },
            new object[] { "Name£!!" }
        };
    }
}
