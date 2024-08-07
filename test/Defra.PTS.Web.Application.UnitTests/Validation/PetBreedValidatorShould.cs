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
    public class PetBreedValidatorShould
    {
        [Fact]
        public async Task NotHaveErrorBreedName()
        {
            var model = new PetBreedViewModel() { BreedName = "Husky" };
            var validator = new PetBreedValidator();

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
            var validator = new PetBreedValidator();

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.BreedId);
        }

        [Fact]
        public async Task HaveErrorIfBreedNameTooLong()
        {
            var model = new PetBreedViewModel()
            {
                PetSpecies = PetSpecies.Dog,
                BreedName = new string('a', 155)
            };
            var validator = new PetBreedValidator();

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
