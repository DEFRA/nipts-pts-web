using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Defra.PTS.Web.Application.Extensions;

namespace Defra.PTS.Web.Application.UnitTests.Extensions
{
    public class IEnumerableExtensionsTests
    {
        [Fact]
        public void WithIndex_ReturnsIndexedSequence_ForEnumerable()
        {
            // Arrange
            var enumerable = new List<string> { "a", "b", "c" };

            // Act
            var result = enumerable.WithIndex().ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal(("a", 0), result[0]);
            Assert.Equal(("b", 1), result[1]);
            Assert.Equal(("c", 2), result[2]);
        }

        [Fact]
        public void WithIndex_ReturnsEmptySequence_ForEmptyEnumerable()
        {
            // Arrange
            var enumerable = new List<string>();

            // Act
            var result = enumerable.WithIndex().ToList();

            // Assert
            Assert.Empty(result);
        }

        [Fact]
        public void WithIndex_ReturnsIndexedSequence_ForEnumerableOfCustomType()
        {
            // Arrange
            var enumerable = new List<int> { 10, 20, 30 };

            // Act
            var result = enumerable.WithIndex().ToList();

            // Assert
            Assert.Equal(3, result.Count);
            Assert.Equal((10, 0), result[0]);
            Assert.Equal((20, 1), result[1]);
            Assert.Equal((30, 2), result[2]);
        }
    }
}
