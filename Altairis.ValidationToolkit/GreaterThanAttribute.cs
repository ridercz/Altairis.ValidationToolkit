using System;
using System.ComponentModel.DataAnnotations;

namespace Altairis.ValidationToolkit {

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class GreaterThanAttribute : ValidationAttribute {
        public GreaterThanAttribute(string otherPropertyName)
            : base("{0} must be greater than {1}") {
            this.OtherPropertyName = otherPropertyName;
        }

        public string OtherPropertyName { get; private set; }

        public string OtherPropertyDisplayName { get; set; }

        public bool AllowEqual { get; set; }

        public override string FormatErrorMessage(string name) {
            return string.Format(this.ErrorMessageString, name, this.OtherPropertyName);
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext) {
            var comparableValue = value as IComparable;
            var comparableOtherValue = validationContext.GetPropertyValue<IComparable>(this.OtherPropertyName);

            // Empty or noncomparable values are valid - let others validate that
            if (comparableValue == null || comparableOtherValue == null) return ValidationResult.Success;

            var compareResult = comparableValue.CompareTo(comparableOtherValue);
            if (compareResult == 1 || (this.AllowEqual && compareResult == 0)) {
                // This property is greater than other property or equal when permitted
                return ValidationResult.Success;
            }
            else {
                // This property is smaller or equal to the other property
                if (string.IsNullOrWhiteSpace(this.OtherPropertyDisplayName)) this.OtherPropertyDisplayName = this.OtherPropertyName;
                return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
            }
        }

        public override bool RequiresValidationContext {
            get {
                return true;
            }
        }

    }
}