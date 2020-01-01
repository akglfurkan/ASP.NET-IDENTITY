using Identity.Identity;
using Identity.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Identity.Controllers
{
    [Authorize]
    public class AccountController : Controller
    {
        private UserManager<ApplicationUser> userManager1;

        public AccountController()
        {
            var userStore = new UserStore<ApplicationUser>(new IdentityDataContext());
            userManager1 = new UserManager<ApplicationUser>(userStore);

            userManager1.PasswordValidator = new CustomPasswordValidator()
            {
                RequireDigit = true,
                RequiredLength = 7

            };
            userManager1.UserValidator = new UserValidator<ApplicationUser>(userManager1)
            {
                RequireUniqueEmail = true,
                AllowOnlyAlphanumericUserNames = false

            };

        }


        // GET: Account
        [AllowAnonymous]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return View("Error", new string[] { "Erisim Hakkınız Yok" });
            }

            ViewBag.returnUrl = returnUrl;
            return View();
        }

        [ValidateAntiForgeryToken]
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            if (ModelState.IsValid)
            {

                var user = userManager1.Find(model.Username, model.Password);
                if (user == null)
                {
                    ModelState.AddModelError("", "Yanlış kullanıcı adı veya parola.");
                }
                else
                {
                    var authManager = HttpContext.GetOwinContext().Authentication;

                    var identity = userManager1.CreateIdentity(user, "ApplicationCookie");

                    var authProperties = new AuthenticationProperties()
                    {
                        IsPersistent = true
                    };

                    authManager.SignOut();
                    authManager.SignIn(authProperties, identity);
                    return Redirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
                }
            }


            ViewBag.returnUrl = returnUrl;
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [AllowAnonymous]
        public ActionResult Register(Register model)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser();

                user.UserName = model.Username;
                user.Email = model.Email;

                var result = userManager1.Create(user, model.Password);

                if (result.Succeeded)
                {
                    userManager1.AddToRole(user.Id, "User");
                    return RedirectToAction("Login");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item);
                    }
                }
            }


            return View(model);
        }

        public ActionResult Logout()
        {
            var authManager = HttpContext.GetOwinContext().Authentication;

            authManager.SignOut();
            return RedirectToAction("Login");


        }

    }
}