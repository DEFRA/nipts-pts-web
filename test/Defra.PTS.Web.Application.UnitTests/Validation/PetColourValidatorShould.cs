using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetColourValidatorShould
    {
        private readonly IStringLocalizer<SharedResource> _localizer;

        public PetColourValidatorShould()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<SharedResource>(factory);
        }

        [Fact]
        public async Task NotHaveErrorPetColour()
        {
            var model = new PetColourViewModel() { PetColour = 1 };
            var validator = new PetColourValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.PetColour);
        }

        [Fact]
        public async Task NotHaveErrorPetColourOther()
        {
            var model = new PetColourViewModel() { PetColourOther = "Pink" };
            var validator = new PetColourValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.PetColourOther);
        }

        [Fact]
        public async Task HaveErrorIfInvalidPetColour()
        {
            var model = new PetColourViewModel() { PetColour = 0 };
            var validator = new PetColourValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.PetColour);
        }

        [Theory]
        [MemberData(nameof(PetColourTestData))]
        public async Task HaveErrorIfInvalidPetColourOther(string petColourOther)
        {
            var model = new PetColourViewModel() 
            { 
                PetColour = 1, 
                OtherColourID = 1, 
                PetColourOther = petColourOther  
            };
            var validator = new PetColourValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.PetColourOther);
        }

        public static IEnumerable<object[]> PetColourTestData => new List<object[]>
        {
            new object[] { string.Empty },
            new object[] { new string('a', 151) }
        };
    }
}
