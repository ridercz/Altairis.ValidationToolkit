using System.Linq;

namespace Altairis.ValidationToolkit {
    public class StaticBankCodeValidator : IBankCodeValidator {
        // Bank codes avaliable from https://www.cnb.cz/cs/platebni-styk/.galleries/ucty_kody_bank/download/kody_bank_CR.csv
        // Valid as of 2021-12-07
        private static readonly string[] BankCodes = {
            "0100", "0300", "0600", "0710", "0800", "2010", "2020", "2060", "2070", "2100",
            "2200", "2220", "2250", "2260", "2275", "2600", "2700", "3030", "3050", "3060",
            "3500", "4000", "4300", "5500", "5800", "6000", "6100", "6200", "6210", "6300",
            "6700", "6800", "7910", "7940", "7950", "7960", "7970", "7990", "8030", "8040",
            "8060", "8090", "8150", "8190", "8198", "8199", "8200", "8220", "8230", "8240",
            "8250", "8255", "8265", "8270", "8272", "8280", "8283", "8293", "8299", "8500" };
        public bool Validate(string code) => BankCodes.Contains(code);
    }
}
