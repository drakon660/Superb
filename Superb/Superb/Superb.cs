using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Whatever;

public class Flatter
{
    public static IReadOnlyDictionary<string, object> ConvertToObjectDictionary(object[] values,
        bool arraySerializeToOneValue = false)
    {
        var result = new Dictionary<string, object>();

        foreach (var value in values)
        {
            if (value is not null)
                Flatten(ref result, value.GetType(), value, arraySerializeToOneValue: arraySerializeToOneValue);
        }

        return result;
    }

    public static string AggregateDictionaryToString(IReadOnlyDictionary<string, object> dictionary)
    {
        return string.Join("-", dictionary.Select(kvp => $"{kvp.Key}-{kvp.Value}"));
    }

    public static string Hash(string input)
    {
        using var sha256 = SHA256.Create();
        var bytes = Encoding.UTF8.GetBytes(input);
        var hash = sha256.ComputeHash(bytes);
        return BitConverter.ToString(hash).Replace("-", "");
    }

    private static void Flatten(ref Dictionary<string, object> flatMap, Type type, object value,
        string propertyName = null, bool arraySerializeToOneValue = false)
    {
        if (value is Array rootArray)
        {
            if (arraySerializeToOneValue)
            {
                var arrayValue = JsonSerializer.Serialize(value, new JsonSerializerOptions
                {
                    PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                    DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                });
                flatMap[type.Name] = arrayValue;
            }
            else
            {
                string arrayName = type.Name.Remove(type.Name.Length - 2);

                for (int i = 0; i < rootArray.Length; i++)
                {
                    var arrayValue = rootArray.GetValue(i);

                    if (arrayValue is not null)
                        Flatten(ref flatMap, arrayValue.GetType(), arrayValue, $"{arrayName}[{i}]");
                }
            }
        }
        else
        {
            foreach (var property in type.GetPropertiesForPublicInstance())
            {
                if (property.IsSimpleType())
                {
                    var key = $"{propertyName ?? type.Name}.{property.Name}";

                    var propValue = property.GetValue(value);

                    if (propValue is DateTime dateTimeValue)
                    {
                        if (dateTimeValue == DateTime.MinValue)
                            continue;

                        propValue = dateTimeValue.ToString("yyyyMMdd");
                    }

                    if (propValue is not null)
                        flatMap[key] = propValue;
                }
                else if (property.PropertyType.IsArray)
                {
                    var propValue = property.GetValue(value);

                    if (propValue is Array array)
                    {
                        if (arraySerializeToOneValue)
                        {
                            var arrayValue = JsonSerializer.Serialize(value, new JsonSerializerOptions
                            {
                                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
                            });
                            
                            flatMap[type.Name] = arrayValue;
                        }
                        else
                        {
                            for (int i = 0; i < array.Length; i++)
                            {
                                var arrayValue = array.GetValue(i);

                                if (arrayValue is not null)
                                    Flatten(ref flatMap, arrayValue.GetType(), arrayValue,
                                        $"{propertyName ?? type.Name}.{property.Name}[{i}]");
                            }
                        }
                    }
                }
                else
                {
                    var propValue = property.GetValue(value);
                    if (propValue is not null)
                        Flatten(ref flatMap, property.PropertyType, propValue,
                            $"{propertyName ?? type.Name}.{property.Name}");
                }
            }
        }
    }
}

// using System;
// using System.Collections.Generic;
// using System.Security.Cryptography;
// using System.Text;
//
// public static class MurmurHash3
// {
//     public static uint ComputeHash(string input)
//     {
//         const uint seed = 0; // Seed value for hash function
//         const uint c1 = 0xcc9e2d51; // Constants for mixing function
//         const uint c2 = 0x1b873593;
//
//         var bytes = Encoding.UTF8.GetBytes(input);
//         var length = (uint)bytes.Length;
//         var remainingBytes = length % 4;
//         var blocks = (uint)(bytes.Length / 4);
//
//         uint hash = seed;
//
//         // Process 4-byte blocks
//         for (int i = 0; i < blocks; i++)
//         {
//             uint block = BitConverter.ToUInt32(bytes, i * 4);
//
//             block *= c1;
//             block = (block << 15) | (block >> 17);
//             block *= c2;
//
//             hash ^= block;
//             hash = (hash << 13) | (hash >> 19);
//             hash = hash * 5 + 0xe6546b64;
//         }
//
//         // Process remaining bytes
//         if (remainingBytes > 0)
//         {
//             uint block = 0;
//             for (int i = 0; i < remainingBytes; i++)
//             {
//                 block |= (uint)bytes[bytes.Length - remainingBytes + i] << (i * 8);
//             }
//
//             block *= c1;
//             block = (block << 15) | (block >> 17);
//             block *= c2;
//
//             hash ^= block;
//         }
//
//         // Finalize hash
//         hash ^= length;
//         hash = MixFinal(hash);
//
//         return hash;
//     }
//
//     private static uint MixFinal(uint hash)
//     {
//         hash ^= hash >> 16;
//         hash *= 0x85ebca6b;
//         hash ^= hash >> 13;
//         hash *= 0xc2b2ae35;
//         hash ^= hash >> 16;
//
//         return hash;
//     }
// }