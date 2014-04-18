using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.ValidationToolkit {
    public class YearOffsetAttribute : ValidationAttribute {

        public YearOffsetAttribute(int beforeCurrent, int afterCurrent)
            : base("{0} must be between {1} and {2}.") {
            if (beforeCurrent < 0) throw new ArgumentOutOfRangeException("beforeCurrent", "Parameter must be positive or zero.");
            if (afterCurrent < 0) throw new ArgumentOutOfRangeException("afterCurrent", "Parameter must be positive or zero.");

            this.MinimumYear = DateTime.Today.Year - beforeCurrent;
            this.MaximumYear = DateTime.Today.Year + afterCurrent;
        }

        public int MinimumYear { get; private set; }

        public int MaximumYear { get; private set; }

        public override bool IsValid(object value) {
            // Convert value to integer
            int intValue;
            try {
                intValue = Convert.ToInt32(value);
            }
            catch (Exception) {
                // Value cannot be processed as int - let other attributes handle that
                return true;
            }

            return intValue >= MinimumYear && intValue <= this.MaximumYear;
        }

        public override string FormatErrorMessage(string name) {
            return string.Format(this.ErrorMessageString, name, this.MinimumYear, this.MaximumYear);
        }

    }
}
