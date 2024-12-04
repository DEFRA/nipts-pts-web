using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;

namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetKeeperPhoneValidatorShould
    {
        private readonly IStringLocalizer<ISharedResource> _localizer;

        public PetKeeperPhoneValidatorShould()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<ISharedResource>(factory);
        }
        [Fact]
        public async Task NotHaveErrorPhoneNumber()
        {
            var model = new PetKeeperPhoneViewModel()
            {
                Phone = "07398141999"
            };

            var validator = new PetKeeperPhoneValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        }

        [Theory]
        [MemberData(nameof(PetKeeperPhoneTestData))]
        public async Task HaveErrorIfPhoneNumberInvalid(string phone)
        {
            var model = new PetKeeperPhoneViewModel()
            {
                Phone = phone
            };

            var validator = new PetKeeperPhoneValidator(_localizer);

            var result = await validator.TestValidateAsync(model);            

            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }

        public static TheoryData<string> PetKeeperPhoneTestData => new TheoryData<string>
        {
            string.Empty,
            new string('a', 51),
            "0739 62345g",
        };
    }
}
