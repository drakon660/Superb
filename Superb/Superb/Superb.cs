using System.Reflection;

namespace Whatever;

public class Flatter2
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

    private static void Flatten(ref Dictionary<string, object> flatMap, Type type, object value,
        string propertyName = null)
    {
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
            {
                if (!string.IsNullOrEmpty(propertyName))
                {
                    string key = $"{propertyName}.{property.Name}";

                    if (value is not null)
                    {
                        var val = property.GetValue(value);
                        flatMap[key] = val;
                    }
                }
                else
                {
                    if (value is not null)
                    {
                        var propValue = property.GetValue(value);
                        if (propValue is not null)
                        {
                            string key = $"{type.Name}.{property.Name}";
                            flatMap[key] = propValue;
                        }
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(propertyName))
                {
                    Flatten(ref flatMap, property.PropertyType, property.GetValue(value),
                        $"{propertyName}.{property.Name}");
                }
                else
                {
                    Flatten(ref flatMap, property.PropertyType, property.GetValue(value),
                        $"{type.Name}.{property.Name}");
                }
            }
        }
    }
}

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
        string[] omitProperties)
    {
        var result = new Dictionary<string, object>();

        foreach (var value in values)
        {
            if (value is not null)
            {
                Flatten(ref result, value.GetType(), value, omitProperties);
            }
        }

        return result;
    }

    private static void Flatten(ref Dictionary<string, object> flatMap, Type type, object value,
        string[] omitProperties, string propertyName = null)
    {
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        foreach (var property in properties)
        {
            if (property.PropertyType.IsValueType || property.PropertyType == typeof(string))
            {
                var key = propertyName != null ? $"{propertyName}.{property.Name}" : $"{type.Name}.{property.Name}";

                if(!omitProperties.Contains(key))
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
                    
                    Flatten(ref flatMap, property.PropertyType, propValue, omitProperties, newPropertyName);
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