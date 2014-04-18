using System;
using System.ComponentModel.DataAnnotations;

namespace Altairis.ValidationToolkit {
    public class DateOffsetAttribute : ValidationAttribute {

        public DateOffsetAttribute(int yearsBeforeCurrent, int yearsAfterCurrent)
            : base("{0} must be between {1:d} and {2:d}.") {

            this.MinimumDate = DateTime.Today.AddYears(yearsAfterCurrent);
            this.MaximumDate = DateTime.Today.AddYears(yearsAfterCurrent);
        }

        public DateOffsetAttribute(TimeSpan beforeCurrent, TimeSpan afterCurrent)
            : base("{0} must be between {1} and {2}.") {

            this.MinimumDate = DateTime.Now.Add(beforeCurrent);
            this.MaximumDate = DateTime.Now.Add(afterCurrent);
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
