using System;
using System.ComponentModel.DataAnnotations;

namespace Altairis.ValidationToolkit {

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateOffsetAttribute : ValidationAttribute {

        public DateOffsetAttribute(int yearsBeforeCurrent, int yearsAfterCurrent, Func<string> errorMessageAccessor)
            : base(errorMessageAccessor) {
            this.MinimumDate = DateTime.Today.AddYears(yearsBeforeCurrent);
            this.MaximumDate = DateTime.Today.AddYears(yearsAfterCurrent);
        }

        public DateOffsetAttribute(int yearsBeforeCurrent, int yearsAfterCurrent, string errorMessage)
            : base(errorMessage) {
            this.MinimumDate = DateTime.Today.AddYears(yearsBeforeCurrent);
            this.MaximumDate = DateTime.Today.AddYears(yearsAfterCurrent);
        }

        public DateOffsetAttribute(string beforeCurrent, string afterCurrent, Func<string> errorMessageAccessor) : base(errorMessageAccessor) {
            TimeSpan beforeCurrentTs = TimeSpan.Zero, afterCurrentTs = TimeSpan.Zero;
            if (!string.IsNullOrEmpty(beforeCurrent) && !TimeSpan.TryParse(beforeCurrent, out beforeCurrentTs)) throw new ArgumentException("String cannot be parsed as TimeSpan.", nameof(beforeCurrent));
            if (!string.IsNullOrEmpty(afterCurrent) && !TimeSpan.TryParse(afterCurrent, out afterCurrentTs)) throw new ArgumentException("String cannot be parsed as TimeSpan.", nameof(afterCurrent));

            this.MinimumDate = DateTime.Now.Add(beforeCurrentTs);
            this.MaximumDate = DateTime.Now.Add(afterCurrentTs);
        }

        public DateOffsetAttribute(string beforeCurrent, string afterCurrent, string errorMessage) : base(errorMessage) {
            TimeSpan beforeCurrentTs = TimeSpan.Zero, afterCurrentTs = TimeSpan.Zero;
            if (!string.IsNullOrEmpty(beforeCurrent) && !TimeSpan.TryParse(beforeCurrent, out beforeCurrentTs)) throw new ArgumentException("String cannot be parsed as TimeSpan.", nameof(beforeCurrent));
            if (!string.IsNullOrEmpty(afterCurrent) && !TimeSpan.TryParse(afterCurrent, out afterCurrentTs)) throw new ArgumentException("String cannot be parsed as TimeSpan.", nameof(afterCurrent));

            this.MinimumDate = DateTime.Now.Add(beforeCurrentTs);
            this.MaximumDate = DateTime.Now.Add(afterCurrentTs);
        }

        public DateOffsetAttribute(int yearsBeforeCurrent, int yearsAfterCurrent)
            : this(yearsBeforeCurrent, yearsAfterCurrent, "{0} must be between {1:d} and {2:d}.") { }

        public DateOffsetAttribute(string beforeCurrent, string afterCurrent)
            : this(beforeCurrent, afterCurrent, "{0} must be between {1:d} and {2:d}.") { }

        public bool CompareTime { get; set; }

        public DateTime MaximumDate { get; private set; }

        public DateTime MinimumDate { get; private set; }

        public override string FormatErrorMessage(string name) => string.Format(this.ErrorMessageString, name, this.MinimumDate, this.MaximumDate);

        public override bool IsValid(object value) {
            // Empty value is always valid
            if (value == null) return true;

            // Convert value to DateTime
            DateTime dateValue;
            try {
                dateValue = Convert.ToDateTime(value);
            } catch (Exception) {
                // Value cannot be processed as DateTime - let other attributes handle that
                return true;
            }

            // Check if value is valid
            if (this.CompareTime) {
                return dateValue >= this.MinimumDate && dateValue <= this.MaximumDate;
            } else {
                return dateValue.Date >= this.MinimumDate.Date && dateValue.Date <= this.MaximumDate.Date;
            }
        }
    }
}