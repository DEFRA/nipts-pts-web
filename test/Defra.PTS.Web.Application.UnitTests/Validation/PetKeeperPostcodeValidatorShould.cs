using Defra.PTS.Web.Application.Features.Address.Queries;
using Defra.PTS.Web.Application.Validation;
using Defra.PTS.Web.Domain;
using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;
using FluentValidation.TestHelper;
using MediatR;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetKeeperPostcodeValidatorShould
    {
        private readonly IStringLocalizer<ISharedResource> _localizer;
        public PetKeeperPostcodeValidatorShould()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<ISharedResource>(factory);
        }

        [Fact]
        public async Task NotHaveErrorPostCode()
        {
            var model = new PetKeeperPostcodeViewModel()
            {
                Postcode = "SW1A 2AA"
            };
            var mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(x => x.Send(It.IsAny<ValidateGreatBritianAddressRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(true));
            var validator = new PetKeeperPostcodeValidator(mockMediator.Object, _localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldNotHaveValidationErrorFor(x => x.Postcode);
        }

        [Theory]
        [MemberData(nameof(PetKeeperPostCodeTestData))]
        public async Task HaveErrorPostCodeInvalid(string postcode, string expectedErrorMessage)
        {
            var model = new PetKeeperPostcodeViewModel()
            {
                Postcode = postcode
            };
            var mockMediator = new Mock<IMediator>();
            mockMediator
                .Setup(x => x.Send(It.IsAny<ValidateGreatBritianAddressRequest>(), It.IsAny<CancellationToken>()))
                .Returns(Task.FromResult(false));
            var validator = new PetKeeperPostcodeValidator(mockMediator.Object, _localizer);

            var result = await validator.TestValidateAsync(model);

            result.ShouldHaveValidationErrorFor(x => x.Postcode).WithErrorMessage(expectedErrorMessage);
        }

        public static IEnumerable<object[]> PetKeeperPostCodeTestData => new List<object[]>
        {
            new object[] { string.Empty, "Enter a postcode" },
            new object[] { "ED34 ER", "Enter a postcode in England, Scotland or Wales" },
            new object[] { new string('a', 21), "Enter a full postcode in the correct format, for example TF7 5AY or TF75AY" },
            new object[] { "SW1A 2AA", "Enter a postcode in England, Scotland or Wales" } // Add this to cover
        };
        }
}