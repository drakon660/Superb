using System.Reflection;

namespace Whatever;

public static class TypeExtensions
{
    public static PropertyInfo[] GetPropertiesForPublicInstance(this Type type) =>
        type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

    public static bool IsSimpleType(this PropertyInfo propertyInfo) =>
        propertyInfo.PropertyType.IsValueType || propertyInfo.PropertyType == typeof(string);
}