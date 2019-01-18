using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Web.Api.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";

            return View();
        }

        public ActionResult SignalR()
        {
            return Content("Comming Soon");
        }

        public ActionResult Contract()
        {
            return Content("Comming Soon");
        }

        public ActionResult Components()
        {
            return Content("Comming Soon");
        }

        public ActionResult Documentation()
        {
            return Content("Comming Soon");
        }
    }
}
