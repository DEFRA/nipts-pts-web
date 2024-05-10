using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetFeatureValidatorShould
    {
        [Fact]
        public async Task NotHaveErrorIfHasUniqueFeature()
        {
            var model = new PetFeatureViewModel() { HasUniqueFeature = YesNoOptions.Yes };
            var validator = new PetFeatureValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.HasUniqueFeature);
        }

        [Fact]
        public async Task NotHaveErrorFeatureDescription()
        {
            var model = new PetFeatureViewModel() { FeatureDescription = "Freckle" };
            var validator = new PetFeatureValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.FeatureDescription);
        }

        [Fact]
        public async Task HaveErrorIfHasUniqueFeatureInvalid()
        {
            var model = new PetFeatureViewModel();
            var validator = new PetFeatureValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.HasUniqueFeature);
        }

        [Theory]
        [MemberData(nameof(PetFeatureTestData))]
        public async Task HaveErrorIfPetFeatureInvalid(string featureDescription)
        {
            var model = new PetFeatureViewModel() 
            { 
                HasUniqueFeature = YesNoOptions.Yes,
                FeatureDescription = featureDescription
            };
            var validator = new PetFeatureValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.FeatureDescription);
        }

        public static IEnumerable<object[]> PetFeatureTestData => new List<object[]>
        {
            new object[] { string.Empty },
            new object[] { new string('a', 301) }
        };
    }
}
