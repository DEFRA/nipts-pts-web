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
    public class PetKeeperUserDetailsValidatorShould
    {
        [Fact]
        public async Task NotHaveErrorUserDetails()
        {
            var model = new PetKeeperUserDetailsViewModel() { UserDetailsAreCorrect = Domain.Enums.YesNoOptions.Yes };
            var validator = new PetKeeperUserDetailsValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.UserDetailsAreCorrect);
        }

        [Fact]
        public async Task HaveErrorIfUserDetailsEmpty()
        {
            var model = new PetKeeperUserDetailsViewModel() { UserDetailsAreCorrect = 0 };
            var validator = new PetKeeperUserDetailsValidator();

            var result = await validator.TestValidateAsync(model);            

            result.ShouldHaveValidationErrorFor(x => x.UserDetailsAreCorrect);
        }
    }
}
