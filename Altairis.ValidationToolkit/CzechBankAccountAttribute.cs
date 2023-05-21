using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Altairis.ValidationToolkit {

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class CzechBankAccountAttribute : DataTypeAttribute {

        private const string AccountNumberFormat = @"^(?:(?<prefix>\d{1,6})-)?(?<number>\d{2,10})/(?<code>\d{4})$";
        private IBankCodeValidator bankCodeValidator = new StaticBankCodeValidator();

        public CzechBankAccountAttribute() : base("CzechBankAccount") {
            this.ErrorMessage = "The field {0} must be a valid Czech bank account number.";
        }

        [Obsolete("Use EmptyBankCodeValidator instead.")]
        public bool IgnoreBankCode { get; set; } = false;

        public override bool IsValid(object value) {
            if (value == null) return true;                             // Null values are valid
            if (!(value is string s)) return false;                     // Non-string values are invalid
            
            var match = Regex.Match(s, AccountNumberFormat);
            if (!match.Success) return false;                           // Unexpected format

            // Split account numer to parts
            var prefix = match.Groups["prefix"].Value;
            var number = match.Groups["number"].Value;
            var bankCode = match.Groups["code"].Value;

            // Validate parts
            bool validatePart(string part, bool isPrefix) {
                if (string.IsNullOrEmpty(part)) return isPrefix;
                if (!isPrefix && int.Parse(part) == 0) return false;

                var chs = 0;
                for (var i = 0; i < part.Length; i++) {
                    var num = int.Parse(part.Substring(i, 1));
                    var weight = (int)Math.Pow(2, part.Length - i - 1) % 11;
                    chs += num * weight;
                }
                return chs % 11 == 0;
            }
            return (this.IgnoreBankCode || this.bankCodeValidator.Validate(bankCode)) && 
                   validatePart(prefix, isPrefix:true) && 
                   validatePart(number, isPrefix: false);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            this.bankCodeValidator = (IBankCodeValidator)validationContext.GetService(typeof(IBankCodeValidator)) ?? this.bankCodeValidator;
            return this.IsValid(value)
                ? ValidationResult.Success
                : new ValidationResult(this.FormatErrorMessage(validationContext.MemberName), new string[] { validationContext.MemberName });
        }
    }
}
