using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.ValidationToolkit {
    public class EmptyBankCodeValidator : IBankCodeValidator {
        public bool Validate(string code) => true;
    }
}
