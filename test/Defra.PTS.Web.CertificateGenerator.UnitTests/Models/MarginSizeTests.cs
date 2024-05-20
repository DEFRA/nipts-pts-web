using System.ComponentModel;
using System.Globalization;

namespace Defra.PTS.Web.CertificateGenerator.UnitTests.Models;


public class MarginSizeTests
{
    [Theory]
    [InlineData(1, 2, 1, 2)]
    [InlineData(1, 2, 3, 4)]
    public static void ConvertFrom_MarginSize(int top, int right, int bottom, int left)
    {
        // arrange
        var sut = new MarginSize(top, right, bottom, left);
        var converter = TypeDescriptor.GetConverter(typeof(MarginSize));

        // act
        _ = converter.CanConvertFrom(typeof(MarginSize)).Should().BeTrue();
        var actual = converter.ConvertFrom(sut);

        // assert
        _ = actual.Should().BeEquivalentTo(sut);
    }

    [Theory]
    [InlineData("5", 5, 5, 5, 5)]
    [InlineData("1,2", 1, 2, 1, 2)]
    [InlineData("1,2,3,4", 1, 2, 3, 4)]
    public static void ConvertFrom_String(string input, int top, int right, int bottom, int left)
    {
        // arrange
        var expected = new MarginSize(top, right, bottom, left);
        var converter = TypeDescriptor.GetConverter(typeof(MarginSize));

        // act
        _ = converter.CanConvertFrom(typeof(string)).Should().BeTrue();
        var actual = converter.ConvertFrom(input);

        // assert
        _ = actual.Should().Be(expected);
    }

    [Fact]
    public static void ConvertFrom_UnsupportedType_ThrowsNotSupportedException()
    {
        // arrange
        var converter = TypeDescriptor.GetConverter(typeof(MarginSize));

        // act
        _ = converter.CanConvertFrom(typeof(DateTime)).Should().BeFalse();
        var test = () => converter.ConvertFrom(DateTime.Now);

        // assert
        _ = test.Should().Throw<NotSupportedException>();
    }

    [Theory]
    [InlineData(1, 2, 1, 2)]
    [InlineData(1, 2, 3, 4)]
    public static void ConvertTo_MarginSize(int top, int right, int bottom, int left)
    {
        // arrange
        var sut = new MarginSize(top, right, bottom, left);
        var converter = TypeDescriptor.GetConverter(typeof(MarginSize));

        // act
        _ = converter.CanConvertTo(typeof(MarginSize)).Should().BeTrue();
        var actual = converter.ConvertTo(sut, typeof(MarginSize));

        // assert
        _ = actual.Should().BeEquivalentTo(sut);
    }

    [Theory]
    [InlineData("1,2,1,2", 1, 2, 1, 2)]
    [InlineData("1,2,3,4", 1, 2, 3, 4)]
    public static void ConvertTo_String(string expected, int top, int right, int bottom, int left)
    {
        // arrange
        var sut = new MarginSize(top, right, bottom, left);
        var converter = TypeDescriptor.GetConverter(typeof(MarginSize));

        // act
        _ = converter.CanConvertTo(typeof(string)).Should().BeTrue();
        var actual = converter.ConvertTo(sut, typeof(string));

        // assert
        _ = actual.Should().Be(expected);
    }

    [Fact]
    public static void ConvertTo_UnsupportedType_ThrowsNotSupportedException()
    {
        // arrange
        var sut = new MarginSize(0);
        var converter = TypeDescriptor.GetConverter(typeof(MarginSize));

        // act
        _ = converter.CanConvertTo(typeof(DateTime)).Should().BeFalse();
        var test = () => converter.ConvertTo(sut, typeof(DateTime));

        // assert
        _ = test.Should().Throw<NotSupportedException>();
    }

    [Fact]
    public void Constructor_Accepts1Arg()
    {
        // arrange
        var size = 10;

        // act
        var sut = new MarginSize(size);

        // assert
        _ = sut.Top.Should().Be(10);
        _ = sut.Left.Should().Be(10);
        _ = sut.Bottom.Should().Be(10);
        _ = sut.Right.Should().Be(10);
    }

    [Fact]
    public void Constructor_Accepts2Args()
    {
        // arrange
        var horizontal = 10;
        var vertical = 20;

        // act
        var sut = new MarginSize(vertical, horizontal);

        // assert
        _ = sut.Top.Should().Be(20);
        _ = sut.Left.Should().Be(10);
        _ = sut.Bottom.Should().Be(20);
        _ = sut.Right.Should().Be(10);
    }

    [Fact]
    public void Constructor_Accepts4Args()
    {
        // arrange
        var top = 1;
        var left = 2;
        var bottom = 3;
        var right = 4;

        // act
        var sut = new MarginSize(top, right, bottom, left);

        // assert
        _ = sut.Top.Should().Be(1);
        _ = sut.Left.Should().Be(2);
        _ = sut.Bottom.Should().Be(3);
        _ = sut.Right.Should().Be(4);
    }

    [Theory]
    [InlineData("5", 5, 5, 5, 5)]
    [InlineData("999", 999, 999, 999, 999)]
    [InlineData("1,2", 1, 2, 1, 2)]
    [InlineData("1,2,3,4", 1, 2, 3, 4)]
    public void TryParse_CorrectlyParses_ValidStringForDefaultNumberStyleAndCulture(string input, int top, int right, int bottom, int left)
    {
        // arrange

        // act
        var success = MarginSize.TryParse(input, out var actual);

        // assert
        _ = success.Should().BeTrue();
        _ = actual.Top.Should().Be(top);
        _ = actual.Right.Should().Be(right);
        _ = actual.Bottom.Should().Be(bottom);
        _ = actual.Left.Should().Be(left);
    }

    [Theory]
    [InlineData("EN-GB", "5", 5, 5, 5, 5)]
    [InlineData("EN-GB", "999", 999, 999, 999, 999)]
    [InlineData("EN-GB", "1,2", 1, 2, 1, 2)]
    [InlineData("EN-GB", "1,2,3,4", 1, 2, 3, 4)]
    public void TryParse_CorrectlyParses_ValidStringForDefaultNumberStyleWithCulture(string culture, string input, int top, int right, int bottom, int left)
    {
        // arrange
        var formatter = CultureInfo.GetCultureInfo(culture);

        // act
        var success = MarginSize.TryParse(input, formatter, out var actual);

        // assert
        _ = success.Should().BeTrue();
        _ = actual.Top.Should().Be(top);
        _ = actual.Right.Should().Be(right);
        _ = actual.Bottom.Should().Be(bottom);
        _ = actual.Left.Should().Be(left);
    }

    [Theory]
    [InlineData("EN-GB", NumberStyles.Integer, "5", 5, 5, 5, 5)]
    [InlineData("EN-GB", NumberStyles.Integer, "999", 999, 999, 999, 999)]
    [InlineData("EN-GB", NumberStyles.Integer, "1,2", 1, 2, 1, 2)]
    [InlineData("EN-GB", NumberStyles.Integer, "1,2,3,4", 1, 2, 3, 4)]
    [InlineData("EN-GB", NumberStyles.Float, "5.0", 5, 5, 5, 5)]
    [InlineData("EN-GB", NumberStyles.Float, "999.0", 999, 999, 999, 999)]
    [InlineData("EN-GB", NumberStyles.Float, "1.0,2.0", 1, 2, 1, 2)]
    [InlineData("EN-GB", NumberStyles.Float, "1.0,2.0,3.0,4.0", 1, 2, 3, 4)]
    public void TryParse_CorrectlyParses_ValidStringWithNumberStyleAndCulture(string culture, NumberStyles style, string input, int top, int right, int bottom, int left)
    {
        // arrange
        var formatter = CultureInfo.GetCultureInfo(culture);

        // act
        var success = MarginSize.TryParse(input, style, formatter, out var actual);

        // assert
        _ = success.Should().BeTrue();
        _ = actual.Top.Should().Be(top);
        _ = actual.Right.Should().Be(right);
        _ = actual.Bottom.Should().Be(bottom);
        _ = actual.Left.Should().Be(left);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("123,456,789")]
    [InlineData("")]
    [InlineData("1,2,3")]
    [InlineData("1.0,2.0,3.0,4.0")]
    [InlineData("1,2,3,4,5")]
    public void TryParse_FailsToParse_InvalidStringForDefaultNumberStyleAndCulture(string input)
    {
        // arrange

        // act
        var success = MarginSize.TryParse(input, out var actual);

        // assert
        _ = success.Should().BeFalse();
        _ = actual.Should().Be(new MarginSize());
    }
}