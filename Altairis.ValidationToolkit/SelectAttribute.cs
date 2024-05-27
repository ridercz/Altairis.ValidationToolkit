using System.ComponentModel.DataAnnotations;

namespace Altairis.ValidationToolkit;

public class SelectAttribute(string listPropertyName = null) : DataTypeAttribute("Select") {

    public string ListPropertyName { get; } = listPropertyName;

}
