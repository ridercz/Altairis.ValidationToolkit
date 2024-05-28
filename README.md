[![NuGet Status](https://img.shields.io/nuget/v/Altairis.ValidationToolkit.svg?style=flat-square&label=nuget)](https://www.nuget.org/packages/Altairis.ValidationToolkit/)

# Altairis Validation Toolkit

Altairis Validation Toolkit strives to make dynamic generation of user interface in ASP.NET Core more powerful and easier. It consists of several types of components, complementing each other:

* **Validation attributes** to validate properties and models in .NET, to extend the ones found in `System.ComponentModel.DataAnnotations` namespace. It has several Czech Republic specific ones (to validate IČO, rodné číslo and Czech bank account number), but others are generally usable, ie. `[RequiredWhen]` or `GreaterThan`.
* **Display attributes** defining some commonly used logical types, like `[Color]`, or user interface elements (like `[Slider]` or `[Select]`).
* **Editor templates** for both standard types and types defined by new attributes above. Also it contains very powerfull `Object.cshtml` template that can render entire forms, including nested complex types and supporting property groups. All templates are clean, modern, semantic HTML5, easy to style using CSS.

> The library is compatible with .NET 8 and above. Last version to support .NET Standard 2.0 is 3.0.1.

## How to install

The best way to install this library is to use the `Altairis.ValidationToolkit` NuGet package for attributes. 

Then dowload the [`EditorTemplates.zip`](https://github.com/ridercz/Altairis.ValidationToolkit/raw/master/dist/EditorTemplates.zip) file and unpack it to appropriate folder:

* For Razor Pages use `~/Pages/EditorTemplates`
* For MVC use `~/Views/Shared/EditorTemplates`

## Documentation

See the [wiki](https://github.com/ridercz/Altairis.ValidationToolkit/wiki) for list of available attributes and templates and their usage. There are also Razor Pages and MVC sample applications showcasing the possibilities.

## License

This library is open source software licensed under terms of the [MIT License](LICENSE.md).

## Contributor Code of Conduct

This project adheres to No Code of Conduct. We are all adults. We accept anyone's contributions. Nothing else matters.

For more information please visit the [No Code of Conduct](https://github.com/domgetter/NCoC) homepage.
