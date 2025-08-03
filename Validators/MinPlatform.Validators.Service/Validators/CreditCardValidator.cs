namespace MinPlatform.Validators.Service.Validators
{
    using MinPlatform.Validators.Service.Definitions;
    using MinPlatform.Validators.Service.Models;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    internal sealed class CreditCardValidator : IValidator<string>
    {
        public ValidationCheckResult IsValid(BaseDefinition typeDefinition, string value)
        {

            var creditCardErrorList = new List<string>();

            string creditCardValue = value.ToString().Replace("-", "").Replace(" ", "");

            int checksum = 0;
            bool evenDigit = false;

            foreach (char digit in creditCardValue.ToCharArray().Reverse())
            {
                if (!char.IsDigit(digit))
                {
                    creditCardErrorList.Add(typeDefinition.GetErrorMessage(ErrorMessageType.InvalidCreditCardFormatMessage) + "-card Number contains letters");

                    break;

                }

                int digitValue = (digit - '0') * (evenDigit ? 2 : 1);
                evenDigit = !evenDigit;

                while (digitValue > 0)
                {
                    checksum += digitValue % 10;
                    digitValue /= 10;
                }
            }

            if ((checksum % 10) != 0)
            {
                creditCardErrorList.Add(typeDefinition.GetErrorMessage(ErrorMessageType.InvalidCreditCardFormatMessage) + "-invalid numbers");
            }

            return new ValidationCheckResult
            {
                ErrorDescription = creditCardErrorList,
                IsValid = !creditCardErrorList.Any()
            };

        }
    }
}
