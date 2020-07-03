[![NuGet Status](https://img.shields.io/nuget/v/Altairis.ValidationToolkit.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Altairis.ValidationToolkit/)

# Altairis Validation Toolkit

Set of various interesting .NET validation attributes, usable for example in ASP.NET MVC, Razor Pages, and Web Forms model binding. You can use them to decorate model and ViewModel properties.

The library is compatible with .NET Standard 2.0.

## How to install

The best way to install this library is to use the `Altairis.ValidationToolkit` NuGet package.

## Validation attributes included

### [CzechBankAccount]

Validates if given string is valid Czech bank account number. Expects value to be in form `prefix-number/bankcode`, where the `prefix-` part is optional.

* `[CzechBankAccount]` - standard usage
* `[CzechBankAccount(IgnoreBankCode = true)]` - will allow any four decimal number as a bank code. See note below.

The code has embedded list of valid bank codes, taken from the Czech National Bank website. The values are fairly static, usually only the company names (which are not used) are changed, not the numbers themselves. But you can turn off validation against this list, if you wish.

### [DateOffset]
Validates if given `DateTime` falls within defined offset from current date. Useful for validating birthdates etc.

* `[DateOffset(-120, 0)]` - date must be between 120 years ago and `DateTime.Today`.
* `[DateOffset(null, "30.00.00.00")]` - date must be between `DateTime.Now` and `DateTime.Now` + 30 days.

*Please note:* By default, the attribute ignores time of day when comparing. If you want to take it in account, set `CompareTime` to `true` and modify error message formatting to show the time of day as well.

### [GreaterThan]
Validates if given value is greater than value of some other property.

* `[GreaterThan("Minimum")]` - value of this property must be greater than value of property `Minimum`.
* `[GreaterThan("Minimum", AllowEqual = true)]` - value of this property must be greater or equal to value of property `Minimum`.

### [Ico]
Validates if given string is valid IČO (*identification number of person*, the state-issued identifier used in Czech Republic).

* `[Ico]` - string must be valid IČO (no options are available)

### [YearOffset]
Validates if given year (expressed as `int`) falls within defined offset from current year. Useful for validating birthdates etc.

* `[YearOffset(-120, 0)]` - year must be between from 120 years ago and current year.
* `[YearOffset(0, 10)]` - year must be in next 10 years from now.

### [RequiredWhen]
Makes property required when some other property has specific value.

* `[RequiredWhen("OtherProperty", "value")]` - property is required when `OtherProperty == "value"`.
* `[RequiredWhen("OtherProperty", "value", NegateCondition = true)]` - property is required when `OtherProperty != "value"`.

*Please note:* The default error message does not mention the master condition (it says *Field {0} is required*, not *Field {0} is required when some conditions are met*). It's recommended to override the message to be more specific to your model.

### [RequiredEmptyWhen]
Complements the `RequiredWhen` attribute. Forces property to have `null` value when other property has specific value.

* `[RequiredEmptyWhen("OtherProperty", "value")]` - property is required to be empty when `OtherProperty == "value"`.
* `[RequiredEmptyWhen("OtherProperty", "value", NegateCondition = true)]` - property is required to be empty when `OtherProperty != "value"`.

*Please note:* The default error message does not mention the master condition (it says *Field {0} is required to be empty*, not *Field {0} is required to be empty when some conditions are met*). It's recommended to override the message to be more specific to your model.

## License

This library is open source software licensed under terms of the [MIT License](LICENSE.md).

## Contributor Code of Conduct

This project adheres to No Code of Conduct. We are all adults. We accept anyone's contributions. Nothing else matters.

For more information please visit the [No Code of Conduct](https://github.com/domgetter/NCoC) homepage.
