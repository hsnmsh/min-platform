namespace MinPlatform.Validators.Service.Validators
{
    using System;
    using System.Collections.Generic;

    internal sealed class CommonValidators
    {
        internal static bool NotNull<Property>(Property value)
        {
            return value != null;
        }

        internal static bool Null<Property>(Property value)
        {
            return value == null;
        }

        internal static bool Empty(string value)
        {
            return value == "";
        }

        internal static bool NotEmpty(string value)
        {
            return string.IsNullOrEmpty(value);
        }

        internal static bool IsMin<Property>(Property value, Property targetValue)
        {
            return Comparer<Property>.Default.Compare(value, targetValue) >= 0;
        }

        internal static bool IsMax<Property>(Property value, Property targetValue)
        {
            return Comparer<Property>.Default.Compare(value, targetValue) <= 0;
        }

        internal static bool Length(int stringLength, int length)
        {
            return Comparer<int>.Default.Compare(stringLength, length) == 0;
        }

        internal static bool StartWith(string value, string valuePart, bool ignoreCase = true)
        {
            return value.StartsWith(valuePart, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        internal static bool EndWith(string value, string valuePart, bool ignoreCase = true)
        {
            return value.EndsWith(valuePart, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        internal static bool Contains(string value, string valuePart, bool ignoreCase = true)
        {
            return value.Contains(valuePart, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal);
        }

        internal static bool ContainsWithIndexOf(string value, string valuePart, int indexOf, bool ignoreCase = true)
        {
            return value.IndexOf(valuePart, indexOf, valuePart.Length, ignoreCase ? StringComparison.OrdinalIgnoreCase : StringComparison.Ordinal)== indexOf;
        }
    }
}
