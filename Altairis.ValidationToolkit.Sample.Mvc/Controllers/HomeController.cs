using Altairis.ValidationToolkit.Sample.Mvc.Models;
using Microsoft.AspNetCore.Mvc;

namespace Altairis.ValidationToolkit.Sample.Mvc.Controllers;

public class HomeController : Controller {

    public ActionResult Index() => this.View();

    [HttpGet]
    public ActionResult ValidationDemo() => this.View(ValidationDemoModel.SampleData);

    [HttpPost]
    public ActionResult ValidationDemo(ValidationDemoModel model) => this.ModelState.IsValid ? this.RedirectToAction(nameof(Index)) : this.View(model);

    [HttpGet]
    public ActionResult EditorTemplatesDemo() => this.View(new EditorTemplatesDemoModel());

    [HttpPost]
    public ActionResult EditorTemplatesDemo(EditorTemplatesDemoModel model) => this.ModelState.IsValid ? this.RedirectToAction(nameof(Index)) : this.View(model);

}