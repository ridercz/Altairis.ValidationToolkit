using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace Altairis.ValidationToolkit.LogicalTypes;

public partial class RodneCislo : IParsable<RodneCislo>, IEquatable<RodneCislo> {

    private string rawValue;

    // Properties

    public DateTime BirthDate { get; private set; }

    public int SequenceNumber { get; private set; }

    public Gender Gender { get; private set; }

    public bool IsExtraSequence { get; private set; }

    // String conversion methods

    public override string ToString() => this.ToString(true);

    public string ToString(bool useSeparator) => useSeparator
            ? string.Join("/", this.rawValue[..6], this.rawValue[6..])
            : this.rawValue;

    // Parse methods

    public static RodneCislo Parse(string s) {
        if (string.IsNullOrWhiteSpace(s)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(s));

        // Remove everything except decimal numbers
        s = RemoveNonNumbersRegex().Replace(s, string.Empty);
        if (s.Length < 9 || s.Length > 10) throw new FormatException("Value must contain 9 or 10 decimal numbers");

        // Create return value
        var r = new RodneCislo {
            rawValue = s
        };

        // Parse year
        var year = int.Parse(s[..2]);
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
    
    public static RodneCislo Parse(string s, IFormatProvider provider) => Parse(s);

    public static bool TryParse(string s, out RodneCislo result) {
        try {
            result = RodneCislo.Parse(s);
            return true;
        } catch (Exception e) when (e is FormatException || e is ArgumentException) {
            result = null;
            return false;
        }
    }
            
    public static bool TryParse([NotNullWhen(true)] string s, IFormatProvider provider, [MaybeNullWhen(false)] out RodneCislo result) => TryParse(s, out result);

    // Implement IEquatable<RodneCislo>

    public bool Equals(RodneCislo other) => other != null && this.rawValue == other.rawValue;

    public override bool Equals(object obj) => this.Equals(obj as RodneCislo);

    public override int GetHashCode() => this.rawValue.GetHashCode();

    // Operators

    public static bool operator ==(RodneCislo left, RodneCislo right) => left?.Equals(right) ?? right is null;

    public static bool operator !=(RodneCislo left, RodneCislo right) => !(left == right);

    [GeneratedRegex(@"[^0-9]")]
    private static partial Regex RemoveNonNumbersRegex();
}

public enum Gender { Male, Female }
