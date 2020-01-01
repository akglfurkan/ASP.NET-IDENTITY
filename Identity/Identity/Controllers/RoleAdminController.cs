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
    [Authorize(Roles = "Administrator")]
    public class RoleAdminController : Controller
    {

        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager1;
        public RoleAdminController()
        {

            userManager1 = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(new IdentityDataContext()));
            roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(new IdentityDataContext()));
        }
        // GET: RoleAdmin
        public ActionResult Index()
        {
            return View(roleManager.Roles);
        }

        [HttpGet]
        public ActionResult Create()
        {



            return View();
        }

        [HttpPost]
        public ActionResult Create(string Name)
        {

            if (ModelState.IsValid)
            {
                var result = roleManager.Create(new IdentityRole(Name));
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var item in result.Errors)
                    {
                        ModelState.AddModelError("", item);
                    }
                }
            }

            return View(Name);
        }

        [HttpPost]
        public ActionResult Delete(string id)
        {
            var role = roleManager.FindById(id);
            if (role != null)
            {
                var result = roleManager.Delete(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    return View("Error", result.Errors);
                }
            }
            else
            {
                return View("Error", new string[] { "Role Bulunamadı" });
            }

        }

        [HttpGet]
        public ActionResult Edit(string id)
        {
            var role = roleManager.FindById(id);
            var members = new List<ApplicationUser>();
            var nonmembers = new List<ApplicationUser>();

            foreach (var users in userManager1.Users.ToList())
            {
                var list = userManager1.IsInRole(users.Id, role.Name) ? members : nonmembers;
                list.Add(users);
            }
            return View(new RoleEditModel()
            {
                Role = role,
                members = members,
                NoNmembers = nonmembers
            });

        }

        [HttpPost]
        public ActionResult Edit(RoleUpdateModel model)
        {
            IdentityResult result;
            if (ModelState.IsValid)
            {
                //ids to add null olursa foreachın hata vermemesi için bir dizi olarak algılaması gerekiyor
                foreach (var item in model.IdsToAdd ?? new string[] { })
                {
                    result = userManager1.AddToRole(item, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                foreach (var item in model.IdsToDelete ?? new string[] { })
                {
                    result = userManager1.RemoveFromRole(item, model.RoleName);
                    if (!result.Succeeded)
                    {
                        return View("Error", result.Errors);
                    }
                }
                return RedirectToAction("Index");
            }


            return View("Error", new string[] { "aranılan rol yok" });
        }

    }
}