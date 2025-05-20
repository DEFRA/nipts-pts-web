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

        [Theory]
        [MemberData(nameof(PetKeeperPhoneSuccessTestData))]
        public async Task NotHaveErrorPhoneNumber(string phone)
        {
            var model = new PetKeeperPhoneViewModel()
            {
                Phone = phone
            };

            var validator = new PetKeeperPhoneValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.Phone);
        }

        [Theory]
        [MemberData(nameof(PetKeeperPhoneErrorTestData))]
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

        public static TheoryData<string> PetKeeperPhoneErrorTestData => new TheoryData<string>
        {
            string.Empty,
            new string('a', 51),
            "0739 62345g",
            "014156667777",
            "0919440973985",
        };

        public static TheoryData<string> PetKeeperPhoneSuccessTestData => new TheoryData<string>
        {
            "07398141999",
            "+447398141999",
            "+14156667777",
            "+919440973985",
            "00919440973985",
            "00447398141999",
            "0014156667777"
        };
    }
}
