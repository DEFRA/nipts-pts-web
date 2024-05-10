using Defra.PTS.Web.Infrastructure.Services.Interfaces;
using Moq;
using NUnit.Framework;
using Address = Defra.PTS.Web.Domain.Models.Address;
using Assert = NUnit.Framework.Assert;

namespace Defra.PTS.Web.Application.UnitTests.Services.Services
{
    [TestFixture]
    public class AddressLookupServiceTests
    {
        protected Mock<HttpMessageHandler> _mockHttpMessageHandler = new();

        [Test]
        public void ConvertAddress_Return_Success()
        {
            var address = new Address
            {
                AddressLineOne = "10 Downing Street",
                AddressLineTwo = "London",
                County = "Greater London",
                Postcode = "SW1A 2AA",
                TownOrCity = "London"
            };

            var csv = "10 Downing Street;London;London;Greater London;SW1A 2AA";
            var addressFromCsv = new Address(csv);
            var csvFromAdddress = address.ToCsvString();

            Assert.AreEqual(address.AddressLineOne, addressFromCsv.AddressLineOne);
            Assert.AreEqual(address.AddressLineTwo, addressFromCsv.AddressLineTwo);
            Assert.AreEqual(address.County, addressFromCsv.County);
            Assert.AreEqual(address.Postcode, addressFromCsv.Postcode);
            Assert.AreEqual(address.TownOrCity, addressFromCsv.TownOrCity);
            Assert.AreEqual(address.ToDisplayString(), addressFromCsv.ToDisplayString());
            Assert.AreEqual(csvFromAdddress, csv);
        }

    }
}
