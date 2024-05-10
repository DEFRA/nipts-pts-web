using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Defra.PTS.Web.Application.Extensions;

namespace Defra.PTS.Web.Application.UnitTests.Extensions
{
    public class DateTimeExtensionsTests
    {
        [Fact]
        public void ToUtcString_ReturnsCorrectUtcString()
        {
            // Arrange
            var dateTime = new DateTime(2022, 3, 21, 10, 30, 0);

            // Act
            var result = dateTime.ToUtcString();

            // Assert
            Assert.Equal("2022-03-21T10:30:00Z", result);
        }

        [Fact]
        public void ToUKDateString_ReturnsCorrectDateString()
        {
            // Arrange
            var dateTime = new DateTime(2022, 3, 21);

            // Act
            var result = dateTime.ToUKDateString();

            // Assert
            Assert.Equal("21/03/2022", result);
        }

        [Fact]
        public void ToUKDateString_NullableDateTime_ReturnsCorrectDateString()
        {
            // Arrange
            DateTime? dateTime = new DateTime(2022, 3, 21);

            // Act
            var result = dateTime.ToUKDateString();

            // Assert
            Assert.Equal("21/03/2022", result);
        }

        [Fact]
        public void ToUKDateString_NullableDateTime_NullValue_ReturnsEmptyString()
        {
            // Arrange
            DateTime? dateTime = null;

            // Act
            var result = dateTime.ToUKDateString();

            // Assert
            Assert.Equal(string.Empty, result);
        }
    }
}
