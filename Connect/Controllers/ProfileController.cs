using Connect.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Connect.Controllers {
    public class ProfileController : Controller {
        LpuContext lpuContext = new LpuContext();
        // GET
        public ActionResult Index() {
            return View();
        }

        // POST: Profile/Search
        [HttpPost]
        [Authorize]
        public JsonResult Search(string searchVal) {
            //create list
            List<User> users = lpuContext.Users.Where(user => user.Username.StartsWith(searchVal) && user.Username != User.Identity.Name).ToArray().Select(user => new User(user)).ToList();
            //return json
            return Json(users);
        }

        [HttpPost]
        [Authorize]
        public void Connect(string friend) {
            //get self id
            User self = lpuContext.Users.Where(x => x.Username.Equals(User.Identity.Name)).FirstOrDefault();
            long selfId = self.UserId;
            //get friend id
            User user = lpuContext.Users.Where(x => x.Username.Equals(friend)).FirstOrDefault();
            long friendId = user.UserId;
            //add connection
            Connection connect = new Connection();
            connect.User_Sender = selfId;
            connect.User_Receiver = friendId;
            connect.Active = false;
            //add and save in db
            lpuContext.Connections.Add(connect);
            lpuContext.SaveChanges();
        }
    }
}