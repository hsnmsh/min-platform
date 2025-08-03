namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class DefinedEnumValidator : IValidator<Enum>
    {
        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, Enum value)
        {
            Type enumType = value.GetType();

            var errorList = new List<string>();
            var underlyingEnumType = Nullable.GetUnderlyingType(enumType) ?? enumType;

            if (!underlyingEnumType.IsEnum)
            {
                errorList.Add(typeDefinition.GetErrorMessage(ErrorMessageType.InvalidEnumMessage));
            }

            if (!IsFlagsEnumDefined(underlyingEnumType, value) || !Enum.IsDefined(underlyingEnumType, value))
            {
                errorList.Add(typeDefinition.GetErrorMessage(ErrorMessageType.UndefinedEnumMessage));
            }

            return new ValidationCheckResult
            {
                ErrorDescription = errorList,
                IsValid = !errorList.Any()
            };

        }

        private static bool IsFlagsEnumDefined(Type enumType, object value)
        {
            var typeName = Enum.GetUnderlyingType(enumType).Name;

            switch (typeName)
            {
                case "Byte":
                    {
                        var typedValue = (byte)value;
                        return EvaluateFlagEnumValues(typedValue, enumType);
                    }

                case "Int16":
                    {
                        var typedValue = (short)value;

                        return EvaluateFlagEnumValues(typedValue, enumType);
                    }

                case "Int32":
                    {
                        var typedValue = (int)value;

                        return EvaluateFlagEnumValues(typedValue, enumType);
                    }

                case "Int64":
                    {
                        var typedValue = (long)value;

                        return EvaluateFlagEnumValues(typedValue, enumType);
                    }

                case "SByte":
                    {
                        var typedValue = (sbyte)value;

                        return EvaluateFlagEnumValues(Convert.ToInt64(typedValue), enumType);
                    }

                case "UInt16":
                    {
                        var typedValue = (ushort)value;

                        return EvaluateFlagEnumValues(typedValue, enumType);
                    }

                case "UInt32":
                    {
                        var typedValue = (uint)value;

                        return EvaluateFlagEnumValues(typedValue, enumType);
                    }

                case "UInt64":
                    {
                        var typedValue = (ulong)value;

                        return EvaluateFlagEnumValues((long)typedValue, enumType);
                    }

                default:
                    var message = $"Unexpected typeName of '{typeName}' during flags enum evaluation.";

                    throw new ValidatorException(message);
            }
        }

        private static bool EvaluateFlagEnumValues(long value, Type enumType)
        {
            long mask = 0;

            foreach (var enumValue in Enum.GetValues(enumType))
            {
                var enumValueAsInt64 = Convert.ToInt64(enumValue);

                if ((enumValueAsInt64 & value) == enumValueAsInt64)
                {
                    mask |= enumValueAsInt64;

                    if (mask == value)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
