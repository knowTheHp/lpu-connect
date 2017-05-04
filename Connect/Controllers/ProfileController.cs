using Connect.Models;
using Connect.Models.ViewModel;
using System.Collections.Generic;
using System.Linq;
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
        public void Connect(string friend) {
            //get user id
            User self = lpuContext.Users.Where(x => x.Username.Equals(User.Identity.Name)).FirstOrDefault();
            long selfId = self.UserId;
            //get friend id
            User user = lpuContext.Users.Where(x => x.Username.Equals(friend)).FirstOrDefault();
            long friendId = user.UserId;

            //add connection
            Connection connect = new Connection() {
                User_Sender = selfId,
                User_Receiver = friendId,
                Active = false
            };

            //add and save in db
            lpuContext.Connections.Add(connect);
            lpuContext.SaveChanges();
        }

        //POST :Profile/FriendRequest
        public JsonResult FriendRequest() {
            lpuContext.Configuration.ProxyCreationEnabled = false;

            //get userid
            User userData = lpuContext.Users.Where(user => user.Username.Equals(User.Identity.Name)).FirstOrDefault();
            long userId = userData.UserId;

            //create list of frn req
            List<FriendRequestVM> friendList = lpuContext.Connections.Where(user => user.User_Receiver == userId && user.Active == false).ToArray().Select(user =>new FriendRequestVM(user)).ToList();

            //init list of user
            List<User> users = new List<User>();
            foreach (FriendRequestVM viewFrndReq in friendList) {
                User user = lpuContext.Users.Where(x => x.UserId == viewFrndReq.Sender).FirstOrDefault();
                users.Add(user);
            }
            //return json
            return Json(users);
        }
    }
}