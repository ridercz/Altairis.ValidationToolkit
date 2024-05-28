using System.Text.RegularExpressions;

namespace Altairis.ValidationToolkit;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed partial class IcoAttribute : DataTypeAttribute {

    public IcoAttribute() : base("Ico") {
        this.ErrorMessage = "The field {0} must be valid IČO (Czech identification number of person).";
    }

    public override bool IsValid(object value) {
        if (value == null) return true;         // Null values are valid
        if (value is not string s) return false; // Non-string values are invalid

        // IČO is 8 digits, pad with zeroes when not
        if (!IcoRegex().IsMatch(s)) return false;
        s = s.PadLeft(8, '0');

        // Calculate sum of digits
        var sum = 0;
        for (var i = 0; i < 7; i++) {
            sum += int.Parse(s[i].ToString()) * (8 - i);
        }

        // Verify checksum number
        var chs = 11 - (sum % 11);
        return chs.ToString().EndsWith(s[7..], StringComparison.Ordinal);
    }

    [GeneratedRegex("^[0-9]{1,8}$")]
    private static partial Regex IcoRegex();
}