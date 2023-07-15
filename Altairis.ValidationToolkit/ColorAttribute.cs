﻿using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace Altairis.ValidationToolkit {

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class ColorAttribute : DataTypeAttribute {

        public ColorAttribute() : base("Color") {
            this.ErrorMessage = "Field {0} must be valid HTML color (#RRGGBB).";
        }

        public override bool IsValid(object value) => value == null || (value is string s && Regex.IsMatch(s, @"^\#[0-9A-Fa-f]{6}"));

    }
}
