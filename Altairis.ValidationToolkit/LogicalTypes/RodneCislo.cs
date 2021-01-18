using System;
using System.Text.RegularExpressions;

namespace Altairis.ValidationToolkit.LogicalTypes {
    public class RodneCislo {
        private string rawValue;

        public DateTime BirthDate { get; private set; }

        public int SequenceNumber { get; private set; }

        public Gender Gender { get; private set; }

        public bool IsExtraSequence { get; private set; }

        public override string ToString() => this.ToString(true);

        public string ToString(bool useSeparator) => useSeparator
                ? string.Join("/", this.rawValue.Substring(0, 6), this.rawValue.Substring(7))
                : this.rawValue;

        public static RodneCislo Parse(string s) {
            if (s == null) throw new ArgumentNullException(nameof(s));
            if (string.IsNullOrWhiteSpace(s)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(s));

            // Remove everything except decimal numbers
            s = Regex.Replace(s, @"[^0-9]", string.Empty);
            if (s.Length < 9 || s.Length > 10) throw new FormatException("Value must contain 9 or 10 decimal numbers");

            // Create return value
            var r = new RodneCislo {
                rawValue = s
            };

            // Parse year
            var year = int.Parse(s.Substring(0, 2));
            if (s.Length == 9) {
                year += year < 54 ? 1900 : 1800;
            } else {
                year += year < 54 ? 2000 : 1900;
            }

            // Parse month
            var month = int.Parse(s.Substring(2, 2));
            if (month >= 1 && month <= 12) {
                r.Gender = Gender.Male;
                r.IsExtraSequence = false;  // first sequence
            } else if (month >= 51 && month <= 62) {
                r.Gender = Gender.Female;
                r.IsExtraSequence = false;  // first sequence
                month -= 50;
            } else if (month >= 21 && month <= 32) {
                r.Gender = Gender.Male;
                r.IsExtraSequence = true;   // second sequence
                month -= 20;
            } else if (month >= 71 && month <= 82) {
                r.Gender = Gender.Female;
                r.IsExtraSequence = true;   // second sequence
                month -= 70;
            } else {
                throw new FormatException("Value contains invalid month.");
            }

            // Parse date
            try {
                r.BirthDate = new DateTime(year, month, int.Parse(s.Substring(4, 2)));
            } catch (ArgumentOutOfRangeException) {
                throw new FormatException("Value contains invalid date.");
            }

            // Sequence number
            r.SequenceNumber = int.Parse(s.Substring(6, 3));

            // Checksum
            return year >= 1954 && long.Parse(s) % 11 > 0 ? throw new FormatException("Value contains invalid checksum") : r;
        }

        public static bool TryParse(string s, out RodneCislo result) {
            try {
                result = RodneCislo.Parse(s);
                return true;
            } catch (Exception e) when (e is FormatException || e is ArgumentException) {
                result = null;
                return false;
            }
        }

    }

    public enum Gender { Male, Female }

}
