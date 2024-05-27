using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Altairis.ValidationToolkit.Sample.RazorPages.Pages;

public class EditorTemplatesDemoModel : PageModel {
    [BindProperty]
    public InputModel Input { get; set; } = new();

    public class InputModel {

        // Common simple types

        public string Name { get; set; } = string.Empty;

        public int Count { get; set; }

        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public bool Option { get; set; }

        [DataType(DataType.Date)]
        public DateTime? Date { get; set; }

        [DataType(DataType.MultilineText)]
        public string MultiLineText { get; set; } = string.Empty;

        // Hidden fields

        [HiddenInput]
        public string HiddenField { get; set; } = "hidden field value";

        [HiddenInput(DisplayValue = false)]
        public string ReallyHiddenField { get; set; } = "really hidden field value";

        // Custom data type attributes

        [Markdown]
        [Display(GroupName = "Custom Data Types")]
        public string Markdown { get; set; } = string.Empty;

        [Select]
        [Display(GroupName = "Custom Data Types")]
        public int Category { get; set; }

        // This property is used to populate the select list and is not bound to the model
        [ScaffoldColumn(false)]
        public IEnumerable<SelectListItem> CategoryList {
            get {
                var group1 = new SelectListGroup() { Name = "Group 1" };
                var group2 = new SelectListGroup() { Name = "Group 2" };
                return [
                    new () { Value = "1", Text = "Category One" },
                new () { Value = "2", Text = "Category Two" },
                new () { Value = "3", Text = "Category Three" },
                new () { Value = "4", Text = "Category 1.1", Group = group1 },
                new () { Value = "5", Text = "Category 1.2", Group = group1 },
                new () { Value = "6", Text = "Category 1.3", Group = group1 },
                new () { Value = "7", Text = "Category 2.1", Group = group2 },
                new () { Value = "8", Text = "Category 2.2", Group = group2 },
                new () { Value = "9", Text = "Category 2.3", Group = group2 }
                ];
            }
        }

        //Range(1, 10)]
        [Slider(1, 10)]
        [Display(GroupName = "Custom Data Types")]
        public int Priority { get; set; } = 5;

        // Complex data types

        [Display(GroupName = "Complex Data Types")]
        public AddressModel RegisteredAddress { get; set; } = new();

        [Display(GroupName = "Complex Data Types")]
        public AddressModel DeliveryAddress { get; set; } = new();

    }

    public class AddressModel {

        public StreetModel Street { get; set; } = new();

        public string City { get; set; } = string.Empty;

        [DataType(DataType.PostalCode)]
        public string Zip { get; set; } = string.Empty;

    }

    public class StreetModel {

        public string StreetName { get; set; } = string.Empty;

        public string StreetNumber { get; set; } = string.Empty;

    }

    public IActionResult OnPost() {
        if (!this.ModelState.IsValid) return this.Page();

        // Save to database etc.

        return this.RedirectToPage("Index");
    }

}
