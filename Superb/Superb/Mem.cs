using System.Reflection;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Whatever;

public static class SizeOfHelper
{
    public static int GetObjectSize<T>()
    {
        if (typeof(T).IsBlittable())
        {
            return Marshal.SizeOf<T>();
        }
        else
        {
            return Unsafe.SizeOf<T>();
        }
    }

    private static bool IsBlittable(this Type type)
    {
        if (type.IsPrimitive || type.IsPointer)
        {
            return true;
        }
        if (type.IsValueType)
        {
            return type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                .All(f => IsBlittable(f.FieldType));
        }
        return false;
    }
}
