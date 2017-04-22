using Connect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connect.Controllers {
    public class ProfileController : Controller {
        LpuContext lpuContext = null;
        // GET
        public ActionResult Index() {
            return View();
        }

        // POST: Profile/Search
        [HttpPost]
        //[Authorize(Roles ="User")]
        public JsonResult Search(string searchVal) {
            if (!User.IsInRole("User")) {
                //Init db
                lpuContext = new LpuContext();
                //create list
                List<User> users = lpuContext.Users.Where(user => user.Username.StartsWith(searchVal) && user.Username != User.Identity.Name).ToArray().Select(user => new User(user)).ToList();
                //return json
                return Json(users);
            } else {
                ViewBag.UserRole = "Please verify your Identity";
            }
            return Json(null);
        }
    }
}