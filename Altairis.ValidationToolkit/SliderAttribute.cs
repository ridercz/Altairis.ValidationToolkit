using System;
using System.ComponentModel.DataAnnotations;

namespace Altairis.ValidationToolkit;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class SliderAttribute(int min, int max, int step = 1) : DataTypeAttribute("Slider") {

    public int Min { get; } = min;

    public int Max { get; } = max;

    public int Step { get; } = step;

    public string ExtraFieldSuffix { get; set; } = "Extra";

}