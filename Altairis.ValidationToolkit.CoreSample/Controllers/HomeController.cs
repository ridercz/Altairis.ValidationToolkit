using Altairis.ValidationToolkit.Sample.Models;
using Microsoft.AspNetCore.Mvc;

namespace Altairis.ValidationToolkit.Sample.Controllers {
    public class HomeController : Controller {

        public ActionResult Index() => this.View(SampleModel.SampleData);

        [HttpPost]
        public ActionResult Index(SampleModel model) => this.ModelState.IsValid ? this.RedirectToAction("Done") : (ActionResult)this.View(model);

        public ActionResult Done() => this.View();


    }
}