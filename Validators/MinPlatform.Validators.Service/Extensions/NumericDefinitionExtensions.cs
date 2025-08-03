namespace MinPlatform.Validators.Service.Extensions
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Validators;
    using System;

    public static class NumericDefinitionExtensions
    {
        public static NumericDefinition LessThan<TNumericType, NumericProperty>(this NumericDefinition decimalDefinition, NumericProperty compareValue)
        {
            decimalDefinition.SetValidator(new LessThanValidator<NumericProperty>(compareValue));

            return decimalDefinition;
        }

        public static NumericDefinition LessThanOrEqual<TNumericType, NumericProperty>(this NumericDefinition decimalDefinition, NumericProperty compareValue)
        {
            decimalDefinition.SetValidator(new LessThanOrEqualValidator<NumericProperty>(compareValue));

            return decimalDefinition;
        }

        public static NumericDefinition GreaterThan<TNumericType, NumericProperty>(this NumericDefinition decimalDefinition, NumericProperty compareValue)
        {
            decimalDefinition.SetValidator(new GreaterThanValidator<NumericProperty>(compareValue));

            return decimalDefinition;
        }

        public static NumericDefinition GreaterThanOrEqual<TNumericType, NumericProperty>(this NumericDefinition decimalDefinition, NumericProperty compareValue)
        {
            decimalDefinition.SetValidator(new GreaterThanOrEqualValidator<NumericProperty>(compareValue));

            return decimalDefinition;
        }

        public static NumericDefinition Min<TNumericType, NumericProperty>(this NumericDefinition decimalDefinition, NumericProperty minValue)
        {
            decimalDefinition.SetValidator(new MinValidator<NumericProperty>(minValue));

            return decimalDefinition;
        }

        public static NumericDefinition Max<TNumericType, NumericProperty>(this NumericDefinition decimalDefinition, NumericProperty maxValue)
        {
            decimalDefinition.SetValidator(new MaxValidator<NumericProperty>(maxValue));

            return decimalDefinition;
        }

        public static NumericDefinition Range<TNumericType, NumericProperty>(this NumericDefinition decimalDefinition, NumericProperty startValue, NumericProperty endValue) 
        { 
            decimalDefinition.SetValidator(new RangeValidator<NumericProperty>(startValue, endValue));

            return decimalDefinition;
        }
    }
}
