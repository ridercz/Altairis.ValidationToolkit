﻿using System;
using System.ComponentModel.DataAnnotations;

namespace Altairis.ValidationToolkit;

[AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
public sealed class MarkdownAttribute() : DataTypeAttribute("Markdown") { }
