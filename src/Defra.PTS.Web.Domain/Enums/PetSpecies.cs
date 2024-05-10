using System.ComponentModel;

namespace Defra.PTS.Web.Domain.Enums;

public enum PetSpecies
{
    [Description("")]
    None = 0,

    [Description("Dog")]
    Dog = 1,

    [Description("Cat")]
    Cat = 2,

    [Description("Ferret")]
    Ferret = 3
}
