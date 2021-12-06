using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;

namespace Altairis.ValidationToolkit {
    public class OnlineBankCodeValidator : IBankCodeValidator {
        private readonly OnlineBankCodeValidatorOptions options;
        private IEnumerable<string> bankCodes;
        private DateTime lastUpdate = DateTime.MinValue;

        public OnlineBankCodeValidator() {
            this.options = new OnlineBankCodeValidatorOptions();
        }

        public OnlineBankCodeValidator(OnlineBankCodeValidatorOptions options) {
            this.options = options ?? throw new ArgumentNullException(nameof(options));
        }

        public TimeSpan ListAge => DateTime.Now.Subtract(this.lastUpdate);

        public bool Validate(string code) {
            // Update bank codes if needed
            this.UpdateBankCodes();

            // Validate code
            return this.bankCodes?.Contains(code) ?? true;
        }

        private void UpdateBankCodes() {
            if (this.ListAge < this.options.MaxAge) return;

            // Download list from ČNB site
            string csv;
            try {
                using (var wc = new TimeoutWebClient(this.options.Timeout)) {
                    csv = wc.DownloadString(this.options.ListUrl);
                }
            } catch (WebException wex) when (wex.Status == WebExceptionStatus.Timeout && !this.options.ThrowExceptionOnTimeout) {
                return;
            } catch (WebException) when (!this.options.ThrowExceptionOnFail) {
                return;
            }

            // Parse the data
            var lines = csv.Split('\r', '\n');
            var list = new List<string>();
            foreach (var item in lines) {
                if (string.IsNullOrEmpty(item)) continue;
                if (Regex.IsMatch(item, @"^\d{4};")) list.Add(item.Substring(0, 4));
            }
            this.bankCodes = list;

            // Set last update time to now
            this.lastUpdate = DateTime.Now;
        }

        private class TimeoutWebClient : WebClient {
            private readonly TimeSpan timeout;

            public TimeoutWebClient(TimeSpan timeout) {
                if (timeout <= TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(timeout));
                this.timeout = timeout;
            }

            protected override WebRequest GetWebRequest(Uri address) {
                var rq = base.GetWebRequest(address);
                rq.Timeout = (int)this.timeout.TotalMilliseconds;
                return rq;
            }
        }

    }

    public class OnlineBankCodeValidatorOptions {

        public TimeSpan MaxAge { get; set; } = TimeSpan.FromDays(1);

        public TimeSpan Timeout { get; set; } = TimeSpan.FromSeconds(5);

        public string ListUrl { get; set; } = "https://www.cnb.cz/cs/platebni-styk/.galleries/ucty_kody_bank/download/kody_bank_CR.csv";

        public bool ThrowExceptionOnTimeout { get; set; } = true;

        public bool ThrowExceptionOnFail { get; set; } = true;

    }
}
