﻿using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;


namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetKeeperNameValidatorShould
    {
        private readonly IStringLocalizer<ISharedResource> _localizer;
        public PetKeeperNameValidatorShould()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<ISharedResource>(factory);
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

        public static TheoryData<string> PetKeeperNameTestData => new TheoryData<string>
        {
            string.Empty,
            new string('a', 301),
        };
    }
}
