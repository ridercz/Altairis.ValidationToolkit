using System;
using System.ComponentModel.DataAnnotations;

namespace Altairis.ValidationToolkit {

    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false)]
    public class DateOffsetAttribute : ValidationAttribute {

        public DateOffsetAttribute(int yearsBeforeCurrent, int yearsAfterCurrent)
            : base("{0} must be between {1:d} and {2:d}.") {

            this.MinimumDate = DateTime.Today.AddYears(yearsBeforeCurrent);
            this.MaximumDate = DateTime.Today.AddYears(yearsAfterCurrent);
        }

        public DateOffsetAttribute(string beforeCurrent, string afterCurrent)
            : base("{0} must be between {1} and {2}.") {

            TimeSpan beforeCurrentTs = TimeSpan.Zero, afterCurrentTs = TimeSpan.Zero;
            if (!string.IsNullOrEmpty(beforeCurrent) && !TimeSpan.TryParse(beforeCurrent, out beforeCurrentTs)) throw new ArgumentException("String cannot be parsed as TimeSpan.", "beforeCurrent");
            if (!string.IsNullOrEmpty(afterCurrent) && !TimeSpan.TryParse(afterCurrent, out afterCurrentTs)) throw new ArgumentException("String cannot be parsed as TimeSpan.", "afterCurrent");

            this.MinimumDate = DateTime.Now.Add(beforeCurrentTs);
            this.MaximumDate = DateTime.Now.Add(afterCurrentTs);
        }

        public DateTime MinimumDate { get; private set; }

        public DateTime MaximumDate { get; private set; }

        public override bool IsValid(object value) {
            // Convert value to DateTime
            DateTime dateValue;
            try {
                dateValue = Convert.ToDateTime(value);
            }
            catch (Exception) {
                // Value cannot be processed as DateTime - let other attributes handle that
                return true;
            }

            // Check if value is valid
            return dateValue >= this.MinimumDate && dateValue <= this.MaximumDate;
        }

        public override string FormatErrorMessage(string name) {
            return string.Format(this.ErrorMessageString, name, this.MinimumDate, this.MaximumDate);
        }

    }
}
