using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace Whatever;

public class Flatter
{
    public static IReadOnlyDictionary<string, object> ConvertToObjectDictionary(object[] values)
    {
        var result = new Dictionary<string, object>();

        foreach (var value in values)
        {
            if (value is not null)
            {
                Flatten(ref result, value.GetType(), value);
            }
        }

        return result;
    }

    public static IReadOnlyDictionary<string, object> ConvertToObjectDictionary(object[] values,
        string[] useProperties)
    {
        var result = new Dictionary<string, object>();

        foreach (var value in values)
        {
            if (value is not null)
            {
                Flatten(ref result, value.GetType(), value, useProperties);
            }
        }

        return result;
    }
    
    public static string AggregateDictionaryToString(IReadOnlyDictionary<string, object> dictionary)
    {
        StringBuilder sb = new StringBuilder();
        foreach (KeyValuePair<string, object> kvp in dictionary)
        {
            sb.Append($"{kvp.Key}-{kvp.Value}");
            sb.Append('-');
        }

        sb.Length -= 1;
        return sb.ToString();
    }
    
    public static string Hash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return BitConverter.ToString(hash).Replace("-", "");
    }
    
    private static void Flatten(ref Dictionary<string, object> flatMap, Type type, object value,
        string[] useProperties, string propertyName = null)
    {
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
            {
                var key = propertyName != null ? $"{propertyName}.{property.Name}" : $"{type.Name}.{property.Name}";

                if(!useProperties.Contains(key))
                    continue;
                
                var propValue = property.GetValue(value);

                if (property.PropertyType == typeof(DateTime)) //TODO omg
                {
                    var dateInString = (propValue as DateTime?).Value.ToString("yyyyMMdd");
                    if (dateInString == DateTime.MinValue.ToString("yyyyMMdd"))
                        propValue = null;
                    else
                        propValue = dateInString;
                }

                if (propValue is not null)
                {
                    flatMap[key] = propValue;
                }
            }
            else
            {
                var propValue = property.GetValue(value);
                if (propValue is not null)
                {
                    var newPropertyName = propertyName != null
                        ? $"{propertyName}.{property.Name}"
                        : $"{type.Name}.{property.Name}";
                    
                    Flatten(ref flatMap, property.PropertyType, propValue, useProperties, newPropertyName);
                }
            }
        }
    }

    private static void Flatten(ref Dictionary<string, object> flatMap, Type type, object value,
        string propertyName = null)
    {
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
            {
                var key = propertyName != null ? $"{propertyName}.{property.Name}" : $"{type.Name}.{property.Name}";

                var propValue = property.GetValue(value);

                if (property.PropertyType == typeof(DateTime)) //TODO omg
                {
                    var dateInString = (propValue as DateTime?).Value.ToString("yyyyMMdd");
                    if (dateInString == DateTime.MinValue.ToString("yyyyMMdd"))
                        propValue = null;
                    else
                        propValue = dateInString;
                }

                if (propValue is not null)
                {
                    flatMap[key] = propValue;
                }
            }
            else
            {
                var propValue = property.GetValue(value);
                if (propValue is not null)
                {
                    var newPropertyName = propertyName != null
                        ? $"{propertyName}.{property.Name}"
                        : $"{type.Name}.{property.Name}";
                    Flatten(ref flatMap, property.PropertyType, propValue, newPropertyName);
                }
            }
        }
    }
}