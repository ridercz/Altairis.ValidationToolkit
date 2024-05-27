[![NuGet Status](https://img.shields.io/nuget/v/Altairis.ValidationToolkit.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Altairis.ValidationToolkit/)

# Altairis Validation Toolkit

Set of various interesting .NET validation attributes, usable for example in ASP.NET MVC, Razor Pages, and Web Forms model binding. You can use them to decorate model and ViewModel properties.

The library is compatible with .NET 8 and above. Last version to support .NET Standard 2.0 is 3.0.1.

## How to install

The best way to install this library is to use the `Altairis.ValidationToolkit` NuGet package.

## Validation attributes included

### [ColorAttribute]

_Editor template name: `Color`._

Validates if given string is a valid HTML color in the `#RRGGBB` syntax.

* `[Color]` - string must ve valid HTML color (no options are available)

### [CzechBankAccount]

_Editor template name: `CzechBankAccount`._

Validates if given string is valid Czech bank account number. Expects value to be in form `prefix-number/bankcode`, where the `prefix-` part is optional.

* `[CzechBankAccount]` - standard usage (no options are available)

#### Validating bank codes

By default, the validator uses hardcoded list of valid bank codes, taken from the Czech National Bank website. The values are fairly static, usually only the company names (which are not used) are changed, not the numbers themselves. But the validation mechanism is extensible.

You can use the `OnlineBankCodeValidator`, which downloads the list of valid codes from CNB website. To do that, register the class to IoC/DI container:

    services.AddSingleton<IBankCodeValidator>(new OnlineBankCodeValidator());

You can also turn off the bank code validation globally by using the `EmptyBankCodeValidator`, which will validate any four digit number:

    services.AddSingleton<IBankCodeValidator>(new EmptyBankCodeValidator());

Additionally, you can create your own validator by implementing the `IBankCodeValidator` interface.

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

_Editor template name: `Ico`._

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

### [RodneCislo]

_Editor template name: `RodneCislo`.__

Validates if a given string is valid rodné číslo ("birth number" the personal identifier of a physical person used in Czech republic). The validation algorithm is derived from § 13 of the Czech law no. 133/2000.

* `[RodneCislo]` - validates the correct format (no options are available).

#### Parsing

There is also class `Altairis.ValidationToolkit.LogicalTypes.RodneCislo` which represents the logical data type.

Use the `Parse` and `TryParse` methods to parse strings as rodné číslo. All characters except decimal numbers are ignored. Instance has the following read-only properties:

* `BirthDate` - date of birth of the person.
* `SequenceNumber` - number discriminating persons born on the same day.
* `Gender` - male or female.
* `IsExtraSequence` - determines where second set of sequence numbers was used.

Instance has the following methods:

* `ToString()` - returns value with `/` separator;
* `ToString(useSeparator: false)` - returns value without the separator.

## License

This library is open source software licensed under terms of the [MIT License](LICENSE.md).

## Contributor Code of Conduct

This project adheres to No Code of Conduct. We are all adults. We accept anyone's contributions. Nothing else matters.

For more information please visit the [No Code of Conduct](https://github.com/domgetter/NCoC) homepage.
