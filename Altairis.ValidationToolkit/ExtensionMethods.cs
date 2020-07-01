using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;

namespace Altairis.ValidationToolkit {

    internal static class ExtensionMethods {

        public static object GetPropertyValue(this ValidationContext validationContext, string propertyName) {
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(propertyName));

            var propertyInfo = validationContext.ObjectType.GetProperty(propertyName);
            if (propertyInfo == null) throw new ArgumentException("Property not found", nameof(propertyName));
            return propertyInfo.GetValue(validationContext.ObjectInstance, null);
        }

        public static T GetPropertyValue<T>(this ValidationContext validationContext, string propertyName) where T : class {
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(propertyName));

            return validationContext.GetPropertyValue(propertyName) as T;
        }

        public static string GetPropertyDisplayName(this ValidationContext validationContext, string propertyName) {
            if (validationContext == null) throw new ArgumentNullException(nameof(validationContext));
            if (propertyName == null) throw new ArgumentNullException(nameof(propertyName));
            if (string.IsNullOrWhiteSpace(propertyName)) throw new ArgumentException("Value cannot be empty or whitespace only string.", nameof(propertyName));

            var property = validationContext.ObjectType.GetRuntimeProperty(propertyName);
            if (property == null) throw new ArgumentException("Property not found", nameof(propertyName));
            var attributes = CustomAttributeExtensions.GetCustomAttributes(property, true);
            var display = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            return display?.GetName() ?? property.Name;
        }

    }
}