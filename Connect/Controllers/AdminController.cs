using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Connect.Models;
namespace Connect.Controllers {
    public class AdminController : Controller {
        LpuContext lpuContext = new LpuContext();
        public ActionResult Index() {
            List<Record> records = lpuContext.Records1.ToList();
            return View(records);
        }

        [HttpPost]
        public ActionResult Verify(long? id) {
            User userDTO = lpuContext.Users.Where(user => user.UserId == id).FirstOrDefault();
            userDTO.IsUserVerified = true;
            userDTO.VerifiedUniqueId = Guid.NewGuid();
            userDTO.VerifiedDateTime = DateTime.Now;
            lpuContext.Users.Add(userDTO);
            lpuContext.SaveChanges();
            return View();
        }
    }
}