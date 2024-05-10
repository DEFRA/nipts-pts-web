using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetKeeperPhoneValidatorShould
    {
        [Fact]
        public async Task NotHaveErrorPhoneNumber()
        {
            var model = new PetKeeperPhoneViewModel()
            {
                Phone = "07398141999"
            };

            var validator = new PetKeeperPhoneValidator();

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

            var validator = new PetKeeperPhoneValidator();

            var result = await validator.TestValidateAsync(model);            

            result.ShouldHaveValidationErrorFor(x => x.Phone);
        }

        public static IEnumerable<object[]> PetKeeperPhoneTestData => new List<object[]>
        {
            new object[] { string.Empty },
            new object[] { new string('a', 51) },
            new object[] { "073962345g" },
            new object[] { "073962345++" },
        };
    }
}
