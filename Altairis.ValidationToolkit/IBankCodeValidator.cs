using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Altairis.ValidationToolkit {
    public interface IBankCodeValidator {
        bool Validate(string code);
    }
}
