using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;


namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetKeeperAddressValidatorShould
    {
        private readonly IStringLocalizer<ISharedResource> _localizer;
        public PetKeeperAddressValidatorShould()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<ISharedResource>(factory);
        }

        [Fact]
        public async Task NotHaveErrorAddress()
        {
            var model = new PetKeeperAddressViewModel() {  Address = "Address 1"};
            var validator = new PetKeeperAddressValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.Address);
        }

        [Fact]
        public async Task NotHaveErrorPostCode()
        {
            var model = new PetKeeperAddressViewModel() { Postcode = "SW1A 2AA" };
            var validator = new PetKeeperAddressValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.Postcode);
        }

        [Fact]
        public async Task HaveErrorIfAddressEmpty()
        {
            var model = new PetKeeperAddressViewModel();
            var validator = new PetKeeperAddressValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }
    }
}
