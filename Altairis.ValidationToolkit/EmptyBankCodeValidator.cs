namespace Altairis.ValidationToolkit {
    public class EmptyBankCodeValidator : IBankCodeValidator {
        public bool Validate(string code) => true;
    }
}
