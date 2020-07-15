using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Altairis.ValidationToolkit {
    public sealed class CzechBankAccountAttribute : ValidationAttribute {

        private const string AccountNumberFormat = @"^(\d{1,6}-)?\d{1,10}/\d{4}$";
        private IBankCodeValidator bankCodeValidator = new StaticBankCodeValidator();

        public CzechBankAccountAttribute() : base("The field {0} must be a valid Czech bank account number.") { }

        public CzechBankAccountAttribute(Func<string> errorMessageAccessor) : base(errorMessageAccessor) { }

        public CzechBankAccountAttribute(string errorMessage) : base(errorMessage) { }

        public bool IgnoreBankCode { get; set; } = false;

        public override bool IsValid(object value) {
            var s = value as string;
            if (string.IsNullOrEmpty(s)) return true;                   // Empty value is always valid
            if (!Regex.IsMatch(s, AccountNumberFormat)) return false;   // Unexpected format

            // Split account numer to parts
            var sd = s.Replace('-', '/').Split('/');
            var prefix = sd.Length == 2 ? string.Empty : sd[0];
            var number = sd[sd.Length - 2];
            var bankCode = sd[sd.Length - 1];

            // Validate parts
            bool validatePart(string part) {
                if (string.IsNullOrEmpty(part)) return true;

                var chs = 0;
                for (var i = 0; i < part.Length; i++) {
                    var num = int.Parse(part.Substring(i, 1));
                    var weight = (int)Math.Pow(2, part.Length - i - 1) % 11;
                    chs += num * weight;
                }
                return chs % 11 == 0;
            }
            return (this.IgnoreBankCode || this.bankCodeValidator.Validate(bankCode)) && validatePart(prefix) && validatePart(number);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            this.bankCodeValidator = (IBankCodeValidator)validationContext.GetService(typeof(IBankCodeValidator)) ?? new StaticBankCodeValidator();
            return this.IsValid(value)
                ? ValidationResult.Success
                : new ValidationResult(this.FormatErrorMessage(validationContext.MemberName), new string[] { validationContext.MemberName });
        }
    }
}
