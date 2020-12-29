using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnimalAdoption.Controllers
{
    [AllowAnonymous]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Animal Adoption description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Animal Adoption contact page.";

            return View();
        }
    }
}