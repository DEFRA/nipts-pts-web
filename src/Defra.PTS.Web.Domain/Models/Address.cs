using System.Text;

namespace Defra.PTS.Web.Domain.Models;

public class Address
{
    public string AddressLineOne { get; set; }

    public string AddressLineTwo { get; set; }

    public string TownOrCity { get; set; }

    public string County { get; set; }

    public string Postcode { get; set; }

    public Address()
    {
    }

    public Address(string fromCsvString)
    {
        var values = fromCsvString.Split(';');
        if (values.Length > 0)
        {
            AddressLineOne = values[0];
        }

        if (values.Length > 1)
        {
            AddressLineTwo = values[1];
        }

        if (values.Length > 2)
        {
            TownOrCity = values[2];
        }

        if (values.Length > 3)
        {
            County = values[3];
        }

        if (values.Length > 4)
        {
            Postcode = values[4];
        }
    }

    public string ToDisplayString()
    {
        var builder = new StringBuilder();

        builder.Append(AddressLineOne);

        if (!string.IsNullOrWhiteSpace(AddressLineTwo))
        {
            builder.Append(", ");
            builder.Append(AddressLineTwo);
        }

        builder.Append(", ");
        builder.Append(TownOrCity);

        if (!string.IsNullOrWhiteSpace(County))
        {
            builder.Append(", ");
            builder.Append(County);
        }

        builder.Append(", ");
        builder.Append(Postcode);

        return builder.ToString();
    }
    public string ToCsvString()
    {
        return $"{AddressLineOne};{AddressLineTwo};{TownOrCity};{County};{Postcode}";
    }
}
