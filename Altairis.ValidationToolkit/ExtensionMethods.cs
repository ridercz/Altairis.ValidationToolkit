using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

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

            var typeDescriptor = new AssociatedMetadataTypeTypeDescriptionProvider(validationContext.ObjectType).GetTypeDescriptor(validationContext.ObjectType);
            var property = typeDescriptor.GetProperties().Find(propertyName, true);
            if (property == null) throw new ArgumentException("Property not found", nameof(propertyName));
            IEnumerable<Attribute> attributes = property.Attributes.Cast<Attribute>();
            DisplayAttribute display = attributes.OfType<DisplayAttribute>().FirstOrDefault();
            if (display != null) {
                return display.GetName();
            }
            var displayName = attributes.OfType<DisplayNameAttribute>().FirstOrDefault();
            if (displayName != null) {
                return displayName.DisplayName;
            }
            return propertyName;

        }

    }
}