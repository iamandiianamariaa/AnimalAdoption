using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace AnimalAdoption.Controllers
{
    public class AdoptionController : Controller
    {
        // GET: Adoption
        public ActionResult Index()
        {
            return View();
        }
    }
}