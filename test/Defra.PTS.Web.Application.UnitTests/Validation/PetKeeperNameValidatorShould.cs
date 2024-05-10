using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using Microsoft.Azure.WebJobs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetKeeperNameValidatorShould
    {
        [Fact]
        public async Task NotHaveErrorName()
        {
            var model = new PetKeeperNameViewModel() { Name = "Name" };
            var validator = new PetKeeperNameValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.Name);
        }

        [Theory]
        [MemberData(nameof(PetKeeperNameTestData))]
        public async Task HaveErrorIfNameInvalid(string name)
        {
            var model = new PetKeeperNameViewModel() { Name = name };
            var validator = new PetKeeperNameValidator();

            var result = await validator.TestValidateAsync(model);

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
