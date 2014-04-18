Altairis Validation Toolkit
===========================

Set of various interesting .NET validation attributes for ASP.NET MVC and Web Forms model binding. You can use them to decorate model and ViewModel properties.

How to install
--------------

The best way to install this library is to use the `Altairis.ValidationToolkit` NuGet package.

Validation attributes included
------------------------------

### DateOffsetAttribute
Validates if given `DateTime` falls within defined offset from current date. Useful for validating birthdates etc.

* `[DateOffset(-120, 0)]` - date must be between 120 years ago and `DateTime.Today`.
* `[DateOffset(null, "30.00.00.00")]` - date must be between `DateTime.Now` and `DateTime.Now` + 30 days.

### GreaterThanAttribute
Validates if given value is greater than value of some other property.

* `[GreaterThan("Minimum")]` - value of this property must be greater than value of property `Minimum`.
* `[GreaterThan("Minimum", AllowEqual = true)]` - value of this property must be greater or equal to value of property `Minimum`.

### IcoAttribute
Validates if given string is valid IČO (_identification number of person_, the state-issued identifier used in Czech Republic).

* `[Ico]` - string must be valid IČO (no options are available)

### YearOffsetAttribute
Validates if given year (expressed as `int`) falls within defined offset from current year. Useful for validating birthdates etc.

* `[YearOffset(-120, 0)]` - year must be between from 120 years ago and current year.
* `[YearOffset(0, 10)]` - year must be in next 10 years from now.

License
-------

This library is open source software licensed under terms of the [MIT License](LICENSE.md).