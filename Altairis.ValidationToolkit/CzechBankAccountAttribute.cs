using System.Text.RegularExpressions;

namespace Altairis.ValidationToolkit;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed partial class CzechBankAccountAttribute : DataTypeAttribute {

    private IBankCodeValidator bankCodeValidator = new StaticBankCodeValidator();

    public CzechBankAccountAttribute() : base("CzechBankAccount") {
        this.ErrorMessage = "The field {0} must be a valid Czech bank account number.";
    }

    public override bool IsValid(object? value) {
        if (value == null) return true;          // Null values are valid
        if (value is not string s) return false; // Non-string values are invalid

        var match = BankAccountNumberFormat().Match(s);
        if (!match.Success) return false; // Unexpected format

        // Split account numer to parts
        var prefix = match.Groups["prefix"].Value;
        var number = match.Groups["number"].Value;
        var bankCode = match.Groups["code"].Value;

        // Validate parts
        static bool validatePart(string part, bool isPrefix) {
            if (string.IsNullOrEmpty(part) || ulong.Parse(part) == 0) return isPrefix;
            var chs = 0;
            for (var i = 0; i < part.Length; i++) {
                var num = int.Parse(part.Substring(i, 1));
                var weight = (int)Math.Pow(2, part.Length - i - 1) % 11;
                chs += num * weight;
            }
            return chs % 11 == 0;
        }
        return validatePart(prefix, isPrefix: true) && validatePart(number, isPrefix: false) && this.bankCodeValidator.Validate(bankCode);
    }

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        this.bankCodeValidator = (IBankCodeValidator?)validationContext.GetService(typeof(IBankCodeValidator)) ?? this.bankCodeValidator;
        return value == null
            ? ValidationResult.Success
            : this.IsValid(value)
            ? ValidationResult.Success
            : (ValidationResult?)new ValidationResult(this.FormatErrorMessage(validationContext.MemberName ?? string.Empty), [validationContext.MemberName ?? string.Empty]);
    }

    [GeneratedRegex(@"^(?:(?<prefix>\d{1,6})-)?(?<number>\d{2,10})/(?<code>\d{4})$")]
    private static partial Regex BankAccountNumberFormat();
}