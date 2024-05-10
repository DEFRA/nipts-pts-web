using System.Diagnostics.CodeAnalysis;

namespace Defra.PTS.Web.Domain.Attributes;

[ExcludeFromCodeCoverage]
[AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = false)]
public sealed class NameAttribute : Attribute
{
    public string Name { get; }

    public NameAttribute(string name)
    {
        Name = name;
    }
}