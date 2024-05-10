using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace Defra.PTS.Web.Testing.Common.Helpers;

[ExcludeFromCodeCoverage]
public static class TestHelper
{
    public static PropertyInfo GetPropertyInfo<T>(string propertyName)
    {
        try
        {
            var memberInfo = typeof(T).GetMember(propertyName)[0];

            if (memberInfo.MemberType == MemberTypes.Property)
            {
                return (PropertyInfo)memberInfo;
            }

            throw new ArgumentException($"Property ({propertyName}) not found");
        }

        catch
        {
            throw new ArgumentException($"Property ({propertyName}) not found");
        }
    }
}
