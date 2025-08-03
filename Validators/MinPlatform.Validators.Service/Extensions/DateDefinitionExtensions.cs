namespace MinPlatform.Validators.Service.Extensions
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Validators;
    using System;

    public static class DateDefinitionExtensions
    {
        public static DateDefinition LessThan(this DateDefinition dateDefinition, DateTime compareValue)
        {
            dateDefinition.SetValidator(new LessThanValidator<DateTime>(compareValue));

            return dateDefinition;
        }

        public static DateDefinition LessThanOrEqual(this DateDefinition dateDefinition, DateTime compareValue)
        {
            dateDefinition.SetValidator(new LessThanOrEqualValidator<DateTime>(compareValue));

            return dateDefinition;
        }

        public static DateDefinition GreaterThan(this DateDefinition dateDefinition, DateTime compareValue)
        {
            dateDefinition.SetValidator(new GreaterThanValidator<DateTime>(compareValue));

            return dateDefinition;
        }

        public static DateDefinition GreaterThanOrEqual(this DateDefinition dateDefinition, DateTime compareValue)
        {
            dateDefinition.SetValidator(new GreaterThanOrEqualValidator<DateTime>(compareValue));

            return dateDefinition;
        }

        public static DateDefinition Min(this DateDefinition dateDefinition, DateTime minDate)
        {
            dateDefinition.SetValidator(new MinValidator<DateTime>(minDate));

            return dateDefinition;
        }

        public static DateDefinition Max(this DateDefinition dateDefinition, DateTime maxDate)
        {
            dateDefinition.SetValidator(new MaxValidator<DateTime>(maxDate));

            return dateDefinition;
        }

        public static DateDefinition Range(this DateDefinition dateDefinition, DateTime startDate, DateTime endDate)
        {
            dateDefinition.SetValidator(new RangeValidator<DateTime>(startDate, endDate));

            return dateDefinition;
        }
    }
}
