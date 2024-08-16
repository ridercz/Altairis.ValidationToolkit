namespace Altairis.ValidationToolkit;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class GreaterThanAttribute : ValidationAttribute {

    public GreaterThanAttribute(string otherPropertyName)
        : this(otherPropertyName, "{0} must be greater than {1}.") { }

    public GreaterThanAttribute(string otherPropertyName, Func<string> errorMessageAccessor)
        : base(errorMessageAccessor) {
        this.OtherPropertyName = otherPropertyName;
        this.OtherPropertyDisplayName = otherPropertyName;
    }

    public GreaterThanAttribute(string otherPropertyName, string errorMessage)
        : base(errorMessage) {
        this.OtherPropertyName = otherPropertyName;
        this.OtherPropertyDisplayName = otherPropertyName;
    }

    public bool AllowEqual { get; set; }

    public string OtherPropertyDisplayName { get; set; }

    public string OtherPropertyName { get; private set; }

    public override bool RequiresValidationContext => true;

    public override string FormatErrorMessage(string name) => string.Format(this.ErrorMessageString, name, this.OtherPropertyDisplayName);

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        // Get values
        IComparable? comparableOtherValue;
        try {
            comparableOtherValue = validationContext.GetPropertyValue<IComparable>(this.OtherPropertyName);
        } catch (ArgumentException aex) {
            throw new InvalidOperationException("Other property not found", aex);
        }

        // Empty or noncomparable values are valid - let others validate that
        if (value is not IComparable comparableValue || comparableOtherValue == null) return ValidationResult.Success;

        var compareResult = comparableValue.CompareTo(comparableOtherValue);
        if (compareResult == 1 || (this.AllowEqual && compareResult == 0)) {
            // This property is greater than other property or equal when permitted
            return ValidationResult.Success;
        } else {
            // This property is smaller or equal to the other property
            if (string.IsNullOrWhiteSpace(this.OtherPropertyDisplayName)) {
                this.OtherPropertyDisplayName = validationContext.GetPropertyDisplayName(this.OtherPropertyName);
            }
            return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
        }
    }

}