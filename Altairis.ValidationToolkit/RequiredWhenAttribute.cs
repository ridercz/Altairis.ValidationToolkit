namespace Altairis.ValidationToolkit;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class RequiredWhenAttribute : ValidationAttribute {

    public RequiredWhenAttribute(string otherPropertyName, object otherPropertyValue, Func<string> errorMessageAccessor) : base(errorMessageAccessor) {
        this.OtherPropertyName = otherPropertyName;
        this.OtherPropertyValue = otherPropertyValue;
    }

    public RequiredWhenAttribute(string otherPropertyName, object otherPropertyValue, string errorMessage) : base(errorMessage) {
        this.OtherPropertyName = otherPropertyName;
        this.OtherPropertyValue = otherPropertyValue;
    }

    public RequiredWhenAttribute(string otherPropertyName, object otherPropertyValue)
        : this(otherPropertyName, otherPropertyValue, "Field {0} is required.") { }

    public bool NegateCondition { get; set; }

    public string OtherPropertyName { get; set; }

    public object OtherPropertyValue { get; set; }

    public override bool RequiresValidationContext => true;

    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext) {
        // Always succeed on non-empty value
        if (value != null) return ValidationResult.Success;

        // Get other property value
        var currentOtherValue = validationContext.GetPropertyValue(this.OtherPropertyName);

        // Compare to it
        if (object.Equals(this.OtherPropertyValue, currentOtherValue) == this.NegateCondition) return ValidationResult.Success;

        // Return error
        return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
    }
}