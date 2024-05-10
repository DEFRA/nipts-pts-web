using Defra.PTS.Web.Domain.Enums;
using Defra.PTS.Web.Domain.Interfaces;
using Defra.PTS.Web.Application.Extensions;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Defra.PTS.Web.Domain.ViewModels.TravelDocument;

namespace Defra.PTS.Web.Application.UnitTests.Extensions
{
    public class IPostcodeModelExtensionsTests
    {
        [Theory]
        [InlineData(PostcodeRegion.GB)]
        public void IsGBPostcode_ReturnsTrue_ForGBRegion(PostcodeRegion region)
        {
            // Arrange
            var model = new PetKeeperAddressViewModel() { PostcodeRegion = region };

            // Act
            var result = model.IsGBPostcode();

            // Assert
            Assert.True(result);
        }

        [Theory]
        [InlineData(PostcodeRegion.NonGB)]
        public void IsGBPostcode_ReturnsFalse_ForNonGBRegion(PostcodeRegion region)
        {
            // Arrange
            var model = new PetKeeperAddressViewModel() { PostcodeRegion = region };

            // Act
            var result = model.IsGBPostcode();

            // Assert
            Assert.False(result);
        }


        [Theory]
        [InlineData(PostcodeRegion.GB)]
        public void IsNonGBPostcode_ReturnsTrue_ForGBRegion(PostcodeRegion region)
        {
            // Arrange
            var model = new PetKeeperAddressViewModel() { PostcodeRegion = region };

            // Act
            var result = model.IsNonGBPostcode();

            // Assert
            Assert.False(result);
        }

        [Theory]
        [InlineData(PostcodeRegion.NonGB)]
        public void IsNonGBPostcode_ReturnsFalse_ForNonGBRegion(PostcodeRegion region)
        {
            // Arrange
            var model = new PetKeeperAddressViewModel() { PostcodeRegion = region };

            // Act
            var result = model.IsNonGBPostcode();

            // Assert
            Assert.True(result);
        }


        // Similar tests for IsNonGBPostcode, IsPostcodeRegionUnknown, and PostcodeStartsWithNonGBPrefix methods
    }




}
