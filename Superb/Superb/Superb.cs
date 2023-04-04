using System.Reflection;
using System.Security.Cryptography.X509Certificates;

namespace Whatever;

public class Flatter
{
    public static IReadOnlyDictionary<string, object> ConvertToObjectDictionary(object[] values)
    {
        var result = new Dictionary<string, object>();

        for (int i = 0; i < values.Length; i++)
        {
            if (values[i] is not null)
            {
                Flatten(ref result, values[i].GetType(), values[i]);
            }
        }

        return result;
    }

    private static void Flatten(ref Dictionary<string, object> flatMap, Type type, object value, string propertyName=null)
    {
        PropertyInfo[] properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);

        for (int j = 0; j < properties.Length; j++)
        {
            if (properties[j].PropertyType.IsValueType || properties[j].PropertyType == typeof(string))
            {
                if (!string.IsNullOrEmpty(propertyName))
                {
                    string key = $"{type.Name}.{propertyName}.{properties[j].Name}";
                    
                    if (value is not null)
                    {
                        var val = properties[j].GetValue(value);
                        flatMap[key] = val;
                    }
                }
                else
                {
                    string key = $"{type.Name}.{properties[j].Name}";
                    flatMap[key] = properties[j].GetValue(value);    
                }
            }
            else
            {
               Flatten(ref flatMap, properties[j].PropertyType, properties[j].GetValue(value), properties[j].Name); 
            }
        }  
    }
}