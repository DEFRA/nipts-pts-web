using Defra.PTS.Web.Domain.Attributes;
using Defra.PTS.Web.Domain.Enums;
using System.ComponentModel;

namespace Defra.PTS.Web.Application.Extensions;

public static class EnumExtensions
{
    public static string GetDescription(this Enum enumValue)
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        if (Attribute.GetCustomAttribute(field, typeof(DescriptionAttribute)) is DescriptionAttribute attribute)
        {
            return attribute.Description;
        }

        return string.Empty;
    }

    public static string GetName(this Enum enumValue)
    {
        var field = enumValue.GetType().GetField(enumValue.ToString());
        if (Attribute.GetCustomAttribute(field, typeof(NameAttribute)) is NameAttribute attribute)
        {
            return attribute.Name;
        }

        return string.Empty;
    }

    public static bool HasBreed(this PetSpecies petSpecies)
    {
        var speciesWithBreeds = new List<PetSpecies>
        {
            PetSpecies.Dog,
            PetSpecies.Cat
        };

        return speciesWithBreeds.Contains(petSpecies);
    }
}
