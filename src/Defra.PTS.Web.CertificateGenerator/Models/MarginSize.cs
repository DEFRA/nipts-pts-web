using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;

namespace Defra.PTS.Web.CertificateGenerator.Models;

[ExcludeFromCodeCoverage]

[TypeConverter(typeof(StringToMarginSizeConverter))]
public record struct MarginSize(int Top, int Right, int Bottom, int Left)
{
    public MarginSize(int size) : this(size, size, size, size)
    {
    }

    public MarginSize(int vertical, int horizontal) : this(vertical, horizontal, vertical, horizontal)
    {
    }

    public static bool TryParse(string value, out MarginSize result) => TryParse(value, NumberStyles.Integer, CultureInfo.CurrentCulture, out result);

    public static bool TryParse(string value, IFormatProvider format, out MarginSize result) => TryParse(value, NumberStyles.Integer, format, out result);

    public static bool TryParse(string value, NumberStyles numberStyles, IFormatProvider format, out MarginSize result)
    {
        var res = TryParse(value, numberStyles, format);
        result = res ?? default;
        return res.HasValue;
    }

    private static MarginSize? TryParse(string value, NumberStyles numberStyles, IFormatProvider format)
    {
        return value?.Split(',') switch
        {
            { Length: 1 } sizeOnly when
                int.TryParse(sizeOnly[0], numberStyles, format, out var size)
                    => new MarginSize(size),
            { Length: 2 } axisOnly when
                int.TryParse(axisOnly[0], numberStyles, format, out var vertical) &&
                int.TryParse(axisOnly[1], numberStyles, format, out var horizontal)
                    => new MarginSize(vertical, horizontal),
            { Length: 4 } sides when
                int.TryParse(sides[0], numberStyles, format, out var top) &&
                int.TryParse(sides[1], numberStyles, format, out var right) &&
                int.TryParse(sides[2], numberStyles, format, out var bottom) &&
                int.TryParse(sides[3], numberStyles, format, out var left)
                    => new MarginSize(top, right, bottom, left),
            _ => null
        };
    }

    public override string ToString() => $"{Top},{Right},{Bottom},{Left}";

    internal class StringToMarginSizeConverter : TypeConverter
    {
        private static readonly Type tString = typeof(string);
        private static readonly Type tMarginSize = typeof(MarginSize);
        private static readonly IReadOnlyCollection<Type> convertable = new HashSet<Type> { tString, tMarginSize };

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType) => convertable.Contains(sourceType);

        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType) => convertable.Contains(destinationType);

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            return value switch
            {
                string str when TryParse(str, culture, out var res) => res,
                MarginSize ms => ms,
                _ => base.ConvertFrom(context, culture, value)
            };
        }

        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            return value switch
            {
                MarginSize ms when destinationType == tString => ms.ToString(),
                MarginSize ms when destinationType == tMarginSize => ms,
                _ => base.ConvertTo(context, culture, value, destinationType)
            };
        }
    }
}