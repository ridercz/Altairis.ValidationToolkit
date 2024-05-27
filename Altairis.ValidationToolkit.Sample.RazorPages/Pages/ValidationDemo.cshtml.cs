using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace Altairis.ValidationToolkit.Sample.RazorPages.Pages;

public class ValidationDemoModel : PageModel {

    // Input model

    [BindProperty]
    public InputModel Input { get; set; } = new InputModel();

    public class InputModel {

        public static readonly InputModel SampleData = new() {
            DateOfBirth = DateTime.Today.AddDays(10),
            NextMonthDate = DateTime.Today.AddDays(-1),
            YearOfBirth = DateTime.Today.Year + 1,
            MinValue = 10,
            MaxValue = 10,
            MaxOrEqualValue = 9,
            Ico = "27560911"
        };

        [DataType(DataType.Date), Display(Name = "Date of birth (must be between 100 years ago and now)")]
        [DateOffset(-100, 0)]
        public DateTime? DateOfBirth { get; set; }

        [DataType(DataType.Date), Display(Name = "Date in next 30 days:")]
        [DateOffset(null, "30.00:00:00")]
        public DateTime? NextMonthDate { get; set; }

        [Display(Name = "Year of birth (must be between 100 years ago and now)")]
        [YearOffset(-100, 0)]
        public int? YearOfBirth { get; set; }

        [Display(Name = "Some minimum value")]
        public int? MinValue { get; set; }

        [Display(Name = "Some value greater than minimum")]
        [GreaterThan("MinValue")]
        public int? MaxValue { get; set; }

        [Display(Name = "Some value greater or equal to minimum")]
        [GreaterThan("MinValue", AllowEqual = true)]
        public int? MaxOrEqualValue { get; set; }

        [Ico]
        public string? Ico { get; set; }

        [Display(Name = "CheckBox value")]
        public bool CheckBox { get; set; }

        [Display(Name = "This value is required when checkbox is checked")]
        [RequiredWhen("CheckBox", true)]
        public string? RequiredWhenChecked { get; set; }

        [Display(Name = "This value is required to be empty when checkbox is checked")]
        [RequiredEmptyWhen("CheckBox", true)]
        public string? RequiredEmptyWhenChecked { get; set; }

        [CzechBankAccount]
        public string? BankAccount { get; set; }

        [Color]
        public string? Color { get; set; }

        [RodneCislo]
        public string? RodneCislo { get; set; }

    }

    // Handlers

    public void OnGetAsync() {
        this.Input = InputModel.SampleData;
    }

    public IActionResult OnPostAsync() {
        if (!this.ModelState.IsValid) return this.Page();

        // Save to database etc.

        return this.RedirectToPage("Index");

    }

}
