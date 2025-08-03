namespace MinPlatform.Validators.Service
{
    public sealed class Constants
    {
        public const string NullMessage = "The value must be Null";
        public const string NotNullMessage = "The value must not be Null";
        public const string NotEqualMessage = "The value must not be Equal to the target value";
        public const string EqualMessage = "The value must be Equal to the target value";
        public const string IncludeMessage = "The value is not within the range";
        public const string InvalidEnumMessage = "The value is not a valid Enum";
        public const string UndefinedEnumMessage = "The enum value is not defined";
        public const string StringEmptyMessage = "The string must be empty";
        public const string StringNotEmptyMessage = "The string must not be empty";
        public const string StringMinLengthMessage = "The string does not have the minimum length";
        public const string StringMaxLengthMessage = "The string passed the maximum length";
        public const string StringInvalidLengthMessage = "value length is not equal  to the required length";
        public const string StringInvalidRegexMessage = "The string does not match the required expression";
        public const string StringStartWithMessage = "value didn't start with the required pattern";
        public const string StringEndWithMessage = "value didn't end with the required pattern";
        public const string StringContainsMessage = "value didn't contain the required pattern";
        public const string EmailMessage = "invalid Email";
        public const string StringContainsWithIndexMessage = "pattern is not positioned in the correct index";
        public const string InvalidCreditCardFormatMessage = "Invalid Credit Card format";
        public const string LessThanMessage = "the value must be less than the target value";
        public const string LessThanOrEqualMessage = "the value must be less or equal than the target value";
        public const string GreaterThanMessage = "the value must be greater than the target value";
        public const string GreaterThanOrEqualMessage = "the value must be greater or equal than the target value";
        public const string MinMessage = "the value is less than the minimum target value";
        public const string MaxMessage = "the value is greater than  maximum target value";
    }

    public enum ErrorMessageType
    {
        NullMessage,
        NotNullMessage,
        NotEqualMessage,
        EqualMessage,
        IncludeMessage,
        InvalidEnumMessage,
        UndefinedEnumMessage,
        StringEmptyMessage,
        StringNotEmptyMessage,
        StringMinLengthMessage,
        StringMaxLengthMessage,
        StringInvalidLengthMessage,
        StringInvalidRegexMessage,
        StringStartWithMessage,
        StringEndWithMessage,
        StringContainsMessage,
        EmailMessage,
        StringContainsWithIndexMessage,
        InvalidCreditCardFormatMessage,
        LessThanMessage,
        LessThanOrEqualMessage,
        GreaterThanMessage,
        GreaterThanOrEqualMessage,
        MinMessage,
        MaxMessage
    }
}
