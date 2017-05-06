using Connect.Models;
using Connect.Models.ViewModel;
using Newtonsoft.Json;
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

        [HttpPost]
        //POST :Profile/FriendRequest
        public ActionResult FriendRequest() {
            //get userId
            User userData = lpuContext.Users.Where(user => user.Username.Equals(User.Identity.Name)).FirstOrDefault();
            long userId = userData.UserId;

            //create list of frn requests
            List<FriendRequestVM> friendList = lpuContext.Connections.Where(user => user.User_Receiver == userId && user.Active == false).ToArray().Select(user => new FriendRequestVM(user)).ToList();

            //init list of user
            List<User> users = new List<User>();
            foreach (FriendRequestVM viewFrndReq in friendList) {
                User user = lpuContext.Users.Where(x => x.UserId == viewFrndReq.Sender).FirstOrDefault();
                users.Add(user);
            }
            string friendRequests = JsonConvert.SerializeObject(users,
            Formatting.None, new JsonSerializerSettings() {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return Content(friendRequests, "application/json");
        }

        [HttpPost]
        public void AcceptRequest(long? friendId) {
            //get userId
            lpuContext = new LpuContext();
            User userObject = lpuContext.Users.Where(user => user.Username.Equals(User.Identity.Name)).FirstOrDefault();
            long UserId = userObject.UserId;

            //Accept request
            Connection connectUsers = lpuContext.Connections.Where(user => user.User_Sender == friendId && user.User_Receiver == UserId).FirstOrDefault();
            connectUsers.Active = true;
            lpuContext.SaveChanges();
        }
    }
}