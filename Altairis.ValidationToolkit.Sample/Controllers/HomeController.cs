using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Altairis.ValidationToolkit.Sample.Models;

namespace Altairis.ValidationToolkit.Sample.Controllers {
    public class HomeController : Controller {

        public ActionResult Index() {
            return this.View();
        }

        public ActionResult Index(SampleModel model) {
            if (this.ModelState.IsValid) return this.RedirectToAction("Done");
            return this.View(model);
        }

        public ActionResult Done() {
            return this.View();
        }


    }
}