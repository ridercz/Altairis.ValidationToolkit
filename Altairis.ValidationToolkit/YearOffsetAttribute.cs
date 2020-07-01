using System;
using System.ComponentModel.DataAnnotations;

namespace Altairis.ValidationToolkit {

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public sealed class YearOffsetAttribute : ValidationAttribute {
        public YearOffsetAttribute(int beforeCurrent, int afterCurrent, Func<string> errorMessageAccessor) : base(errorMessageAccessor) {
            this.MinimumYear = DateTime.Today.Year + beforeCurrent;
            this.MaximumYear = DateTime.Today.Year + afterCurrent;
        }

        public YearOffsetAttribute(int beforeCurrent, int afterCurrent, string errorMessage) : base(errorMessage) {
            this.MinimumYear = DateTime.Today.Year + beforeCurrent;
            this.MaximumYear = DateTime.Today.Year + afterCurrent;
        }

        public YearOffsetAttribute(int beforeCurrent, int afterCurrent)
            : this(beforeCurrent, afterCurrent, "{0} must be between {1} and {2}.") { }

        public int MaximumYear { get; private set; }

        public int MinimumYear { get; private set; }
        public override string FormatErrorMessage(string name) => string.Format(this.ErrorMessageString, name, this.MinimumYear, this.MaximumYear);

        public override bool IsValid(object value) {
            // Empty value is always valid
            if (value == null) return true;

            // Convert value to integer
            int intValue;
            try {
                intValue = Convert.ToInt32(value);
#pragma warning disable CA1031 // Do not catch general exception types
            } catch (Exception) {
                // Value cannot be processed as int - let other attributes handle that
                return true;
            }
#pragma warning restore CA1031 // Do not catch general exception types

            return intValue >= this.MinimumYear && intValue <= this.MaximumYear;
        }
    }
}