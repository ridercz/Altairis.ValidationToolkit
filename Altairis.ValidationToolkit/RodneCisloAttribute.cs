using Altairis.ValidationToolkit.LogicalTypes;

namespace Altairis.ValidationToolkit;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class RodneCisloAttribute : DataTypeAttribute {

    public RodneCisloAttribute() : base("RodneCislo") {
        this.ErrorMessage = "The field {0} must be valid rodné číslo (Czech personal identification number).";
    }

    public override bool IsValid(object value) {
        if (value == null) return true;         // Null values are valid
        if (value is not string s) return false; // Non-string values are invalid

        return RodneCislo.TryParse(s, out _);
    }
}
