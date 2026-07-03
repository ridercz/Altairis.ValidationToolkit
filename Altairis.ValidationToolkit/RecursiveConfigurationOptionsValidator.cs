using System.Collections;
using System.Reflection;
using Microsoft.Extensions.Options;

namespace Altairis.ValidationToolkit;

public class RecursiveConfigurationOptionsValidator<T> : IValidateOptions<T> where T : class {

    public ValidateOptionsResult Validate(string? name, T? options) {
        if (options is null) return ValidateOptionsResult.Fail("Options instance is null");

        var results = new List<ValidationResult>();
        var visited = new HashSet<object>();

        ValidateObjectRecursive(options, results, visited);

        if (results.Count == 0) return ValidateOptionsResult.Success;

        var errors = results.Select(r => r.ErrorMessage ?? r.ToString() ?? "Unknown validation error");
        return ValidateOptionsResult.Fail(errors);
    }

    private static void ValidateObjectRecursive(object obj, List<ValidationResult> results, HashSet<object> visited) {
        if (obj is null) return;

        // Avoid cycles by keeping track of visited objects
        if (visited.Contains(obj)) return;
        visited.Add(obj);

        // Skip primitive types, strings and enums (they can't have nested properties)
        var type = obj.GetType();
        if (type.IsPrimitive || type == typeof(string) || type.IsEnum) return;

        // If it's a collection, validate each element
        if (obj is IEnumerable enumerable && obj is not string) {
            foreach (var item in enumerable) {
                if (item is null) continue;
                ValidateObjectRecursive(item, results, visited);
            }
            return;
        }

        // Validate this object
        var context = new ValidationContext(obj);
        Validator.TryValidateObject(obj, context, results, validateAllProperties: true);

        // Recurse into properties
        var properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(p => p.CanRead && p.GetIndexParameters().Length == 0);
        foreach (var prop in properties) {
            var value = prop.GetValue(obj);
            if (value is null) continue;
            ValidateObjectRecursive(value, results, visited);
        }
    }

}
