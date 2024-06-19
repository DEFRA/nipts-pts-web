using Defra.PTS.Web.Application.Features.Address.Queries;
using Defra.PTS.Web.Application.Validation;
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
using System.Threading.Tasks;

namespace Defra.PTS.Web.Application.UnitTests.Validation
{
    public class PetKeeperPostcodeValidatorShould
    {
        private readonly IStringLocalizer<PetKeeperPostcodeViewModel> _localizer;
        public PetKeeperPostcodeValidatorShould()
        {
            var options = Options.Create(new LocalizationOptions { ResourcesPath = "Resources" });
            var factory = new ResourceManagerStringLocalizerFactory(options, NullLoggerFactory.Instance);
            _localizer = new StringLocalizer<PetKeeperPostcodeViewModel>(factory);
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
        public async Task HaveErrorPostCodeInvalid(string postcode)
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

            result.ShouldHaveValidationErrorFor(x => x.Postcode);
        }

        public static IEnumerable<object[]> PetKeeperPostCodeTestData => new List<object[]>
        {
            new object[] { string.Empty },
            new object[] { "ED34 ER" },
            new object[] { new string('a', 21) },
        };
            
    }
}
