using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Localization;
using Defra.PTS.Web.Domain;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;



namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetKeeperAddressManualValidatorShould
    {
        private readonly IStringLocalizer<ISharedResource> _localizer;
        public PetKeeperAddressManualValidatorShould()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<ISharedResource>(factory);
        }

        [Fact]
        public async Task NotHaveErrorIfPetKeeperAddressManual()
        {
            var model = new PetKeeperAddressManualViewModel()
            {
                AddressLineOne = "street 1",
                AddressLineTwo = "Apartment 2",
                TownOrCity = "London",
                County = "County 1",
                Postcode = "SW1A 2AA"
            };
            var validator = new PetKeeperAddressManualValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.AddressLineOne);
            result.ShouldNotHaveValidationErrorFor(x => x.AddressLineTwo);
            result.ShouldNotHaveValidationErrorFor(x => x.TownOrCity);
            result.ShouldNotHaveValidationErrorFor(x => x.County);
            result.ShouldNotHaveValidationErrorFor(x => x.Postcode);
        }

        [Theory]
        [MemberData(nameof(PetKeeperAddressManualAddressLine1TestData))]
        public async Task HaveErrorIfAddressLine1Invalid(string addressLine1)
        {
            var model = new PetKeeperAddressManualViewModel() {  AddressLineOne = addressLine1 };
            var validator = new PetKeeperAddressManualValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.AddressLineOne);            
        }

        [Theory]
        [MemberData(nameof(PetKeeperAddressManualAddressLine2TestData))]
        public async Task HaveErrorIfAddressLine2Invalid(string addressLine2)
        {
            var model = new PetKeeperAddressManualViewModel() { AddressLineTwo = addressLine2 };
            var validator = new PetKeeperAddressManualValidator(_localizer);

            var result = await validator.TestValidateAsync(model);
            
            result.ShouldHaveValidationErrorFor(x => x.AddressLineTwo);            
        }

        [Theory]
        [MemberData(nameof(PetKeeperAddressManualTownOrCityTestData))]
        public async Task HaveErrorIfTownOrCityInvalid(string townOrCity)
        {
            var model = new PetKeeperAddressManualViewModel() { TownOrCity = townOrCity };
            var validator = new PetKeeperAddressManualValidator(_localizer);

            var result = await validator.TestValidateAsync(model);
            
            result.ShouldHaveValidationErrorFor(x => x.TownOrCity);
        }

        [Theory]
        [MemberData(nameof(PetKeeperAddressManualPostCodeTestData))]
        public async Task HaveErrorIfPostCodeInvalid(string postcode)
        {
            var model = new PetKeeperAddressManualViewModel() { Postcode = postcode };
            var validator = new PetKeeperAddressManualValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.Postcode);
        }

        [Theory]
        [MemberData(nameof(PetKeeperAddressManualCountyTestData))]
        public async Task HaveErrorIfCountyInvalid(string county)
        {
            var model = new PetKeeperAddressManualViewModel() { County = county };
            var validator = new PetKeeperAddressManualValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.County);
        }

        public static IEnumerable<object[]> PetKeeperAddressManualAddressLine1TestData => new List<object[]>
        {            
            new object[] { string.Empty },
            new object[] { new string('a', 251) }
        };

        public static IEnumerable<object[]> PetKeeperAddressManualAddressLine2TestData => new List<object[]>
        {            
            new object[] { new string('a', 251) }
        };

        public static IEnumerable<object[]> PetKeeperAddressManualTownOrCityTestData => new List<object[]>
        {
            new object[] { string.Empty },
            new object[] { new string('a', 251) }
        };

        public static IEnumerable<object[]> PetKeeperAddressManualPostCodeTestData => new List<object[]>
        {
            new object[] { string.Empty },
            new object[] { new string('a', 21) }
        };

        public static IEnumerable<object[]> PetKeeperAddressManualCountyTestData => new List<object[]>
        {            
            new object[] { new string('a', 101) }
        };
    }
}
