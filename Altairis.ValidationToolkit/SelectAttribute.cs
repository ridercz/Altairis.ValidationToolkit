namespace Altairis.ValidationToolkit;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class SelectAttribute(string? listPropertyName = null) : DataTypeAttribute("Select") {

    public string? ListPropertyName { get; } = listPropertyName;

}
