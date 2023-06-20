﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Altairis.ValidationToolkit {

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class CzechBankAccountAttribute : DataTypeAttribute {

        private const string AccountNumberFormat = @"^(\d{1,6}-)?\d{1,10}/\d{4}$";
        private IBankCodeValidator bankCodeValidator = new StaticBankCodeValidator();

        public CzechBankAccountAttribute() : base("CzechBankAccount") {
            this.ErrorMessage = "The field {0} must be a valid Czech bank account number.";
        }

        [Obsolete("Use EmptyBankCodeValidator instead.")]
        public bool IgnoreBankCode { get; set; } = false;

        public override bool IsValid(object value) {
            if (value == null) return true;                             // Null values are valid
            if (!(value is string s)) return false;                     // Non-string values are invalid
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
#pragma warning disable CS0618 // Type or member is obsolete
            return (this.IgnoreBankCode || this.bankCodeValidator.Validate(bankCode)) && validatePart(prefix) && validatePart(number);
#pragma warning restore CS0618 // Type or member is obsolete
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            this.bankCodeValidator = (IBankCodeValidator)validationContext.GetService(typeof(IBankCodeValidator)) ?? this.bankCodeValidator;
            return this.IsValid(value)
                ? ValidationResult.Success
                : new ValidationResult(this.FormatErrorMessage(validationContext.MemberName), new string[] { validationContext.MemberName });
        }
    }
}
