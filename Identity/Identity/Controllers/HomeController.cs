using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Identity.Controllers
{


    public class HomeController : Controller
    {
        // GET: Home
        
        public ActionResult Index()
        {


            return View();
        }

        [Authorize]
        public ActionResult About()
        {


            return View();
        }

    }
}