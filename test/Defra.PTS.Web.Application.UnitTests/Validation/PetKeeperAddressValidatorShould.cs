using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetKeeperAddressValidatorShould
    {
        [Fact]
        public async Task NotHaveErrorAddress()
        {
            var model = new PetKeeperAddressViewModel() {  Address = "Address 1"};
            var validator = new PetKeeperAddressValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.Address);
        }

        [Fact]
        public async Task NotHaveErrorPostCode()
        {
            var model = new PetKeeperAddressViewModel() { Postcode = "SW1A 2AA" };
            var validator = new PetKeeperAddressValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.Postcode);
        }

        [Fact]
        public async Task HaveErrorIfAddressEmpty()
        {
            var model = new PetKeeperAddressViewModel();
            var validator = new PetKeeperAddressValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.Address);
        }

        [Fact]
        public async Task HaveErrorIfPostCodeInvalid()
        {
            var model = new PetKeeperAddressViewModel() { Postcode = "IM13 4RT" };
            var validator = new PetKeeperAddressValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.Postcode);
        }
    }
}
