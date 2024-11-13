using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain.Enums;
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
    public class PetBreedValidatorShould
    {
        private readonly IStringLocalizer<ISharedResource> _localizer;
        public PetBreedValidatorShould()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<ISharedResource>(factory);
        }

        [Fact]
        public async Task NotHaveErrorBreedName()
        {
            var model = new PetBreedViewModel() { BreedName = "Husky" };
            var validator = new PetBreedValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.BreedName);
        }

        [Theory]
        [MemberData(nameof(PetBreedTestData))]
        public async Task HaveErrorIfBreedIdEmpty(PetSpecies petSpecies, string breedName)
        {
            var model = new PetBreedViewModel() 
            { 
                PetSpecies = petSpecies,
                BreedName = breedName
            };
            var validator = new PetBreedValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.BreedName);
        }

        [Fact]
        public async Task HaveErrorIfBreedNameTooLong()
        {
            var model = new PetBreedViewModel()
            {
                PetSpecies = PetSpecies.Dog,
                BreedName = new string('a', 155)
            };
            var validator = new PetBreedValidator(_localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.BreedName);
        }

        public static IEnumerable<object[]> PetBreedTestData => new List<object[]>()
        {
            new object[] { PetSpecies.Dog, null },
            new object[] { PetSpecies.Cat, null },
            new object[] { PetSpecies.Ferret, null },
        };
    }
}
