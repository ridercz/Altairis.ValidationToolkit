using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Altairis.ValidationToolkit {
    public class CzechBankAccountAttribute : ValidationAttribute {
        // Bank codes avaliable from https://www.cnb.cz/cs/platebni-styk/.galleries/ucty_kody_bank/download/kody_bank_CR.csv
        // Valid as of 2020-07-01
        private static readonly string[] BankCodes = {
            "0100","0300","0600","0710","0800","2010","2020","2030","2060","2070",
            "2100","2200","2220","2240","2250","2260","2275","2600","2700","3030",
            "3050","3060","3500","4000","4300","5500","5800","6000","6100","6200",
            "6210","6300","6700","6800","7910","7940","7950","7960","7970","7980",
            "7990","8030","8040","8060","8090","8150","8190","8198","8199","8200",
            "8215","8220","8225","8230","8240","8250","8255","8260","8265","8270",
            "8272","8280","8283","8291","8292","8293","8294","8296" };
        private const string AccountNumberFormat = @"^(\d{1,6}-)?\d{1,10}/\d{4}$";

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
                    var weight = (int)Math.Pow(2, part.Length - i) % 11;
                    chs += num * weight;
                }
                return chs % 11 == 0;
            }
            return (this.IgnoreBankCode || BankCodes.Contains(bankCode)) && validatePart(prefix) && validatePart(number);
        }
    }
}
