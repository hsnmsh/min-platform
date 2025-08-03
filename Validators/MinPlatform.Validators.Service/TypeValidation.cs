using System;

namespace MinPlatform.Validators.Service
{
    internal sealed class TypeValidation
    {
        internal static bool IsNumericType(Type type)
        {
            return type == typeof(int) || type == typeof(float) || type == typeof(double) || type == typeof(decimal) || type == typeof(decimal?) ||
                type == typeof(double?) || type == typeof(float?) || type == typeof(int?) ||
               type == typeof(byte) || type == typeof(short) || type == typeof(long) ||
               type == typeof(uint) || type == typeof(ushort) || type == typeof(ulong);
        }
    }
}
