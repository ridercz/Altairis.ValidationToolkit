using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Altairis.ValidationToolkit; 

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed partial class ColorAttribute : DataTypeAttribute {

    public ColorAttribute() : base("Color") {
        this.ErrorMessage = "Field {0} must be valid HTML color (#RRGGBB).";
    }

    public override bool IsValid(object value) => value == null || (value is string s && ColorRegex().IsMatch(s));
    [GeneratedRegex(@"^\#[0-9A-Fa-f]{6}")]
    private static partial Regex ColorRegex();
}
