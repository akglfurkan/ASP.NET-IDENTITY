using Identity.Identity;
using Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Identity.Controllers
{
    public class AdminController : Controller
    {
        private UserManager<ApplicationUser> userManager1;

        public AdminController()
        {
            var userStore = new UserStore<ApplicationUser>(new IdentityDataContext());
            userManager1 = new UserManager<ApplicationUser>(userStore);
        }

        // GET: Admin
        public ActionResult Index()
        {

            return View(userManager1.Users);
        }
    }
}