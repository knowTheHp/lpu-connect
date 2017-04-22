using Connect.Models;
using System;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace Connect.Controllers {
    public class AccountController : Controller {
        LpuContext lpuContext;
        // GET: /
        public ActionResult Index() {
            //confirm user not logged in 
            string Username = User.Identity.Name;
            if (!String.IsNullOrEmpty(Username)) {
                return Redirect($"~/{Username}");
            } else {
                //return view
                return View();
            }
        }

        //POST: Account/CreateAccount
        [HttpPost]
        public ActionResult CreateAccount(User userModel, HttpPostedFileBase file) {
            try {

                //step 1: Initialize the database
                lpuContext = new LpuContext();

                //step 2: Check model state
                if (!ModelState.IsValid) {
                    return View("Index", userModel);
                }
                //step 3: check if Username is unique
                if (lpuContext.Users.Any(user => user.Username.Equals(userModel.Username))) {
                    ModelState.AddModelError("Username", "Username already taken");
                    userModel.Username = "";
                    return View("Index", userModel);
                }
                //step 4: check if Email is unique
                else if (lpuContext.Users.Any(user => user.Email.Equals(userModel.Email))) {
                    ModelState.AddModelError("Email", "Email already exists");
                    userModel.Email = "";
                    return View("Index", userModel);
                } else {
                    //
                }
                //step 5: Create UserDTO object
                User userDTO = new User() {
                    Firstname = userModel.Firstname,
                    Lastname = userModel.Lastname,
                    Username = userModel.Username,
                    Email = userModel.Email,
                    Password = FormsAuthentication.HashPasswordForStoringInConfigFile(userModel.Password, "SHA1")
                };
                //step 6: Add to DTO
                lpuContext.Users.Add(userDTO);

                //step 7: Set upload directory
                DirectoryInfo uploadDir = new DirectoryInfo(string.Format("{0}Uploads", Server.MapPath(@"\")));

                //Step 8: Check if the file was uploaded
                if (file != null && file.ContentLength > 0) {

                    //Step 9: Get extension
                    string extension = file.ContentType.ToLower();

                    //Step 10: Verify extension
                    if (extension != "image/jpg" && extension != "image/jpeg") {
                        ModelState.AddModelError("", "Image extension invalid");
                        return View("Index", userModel);
                    }
                    //Step 11: Check InputStream
                    WebImage wi = null;
                    try {
                        wi = new WebImage(file.InputStream);
                    } catch (Exception ex) {
                        ModelState.AddModelError("", "Invalid image");
                        return View("Index", userModel);
                    }
                    //Step 12: Set Size
                    if (wi.Width > 800) {
                        wi.Resize(800, 800, false, true);
                    } else {
                        ModelState.AddModelError("", "Image invalid");
                        return View("Index", userModel);
                    }

                    //step 13: Save
                    lpuContext.SaveChanges();

                    //Step 14: Get the inserted id
                    long userId = userDTO.UserId;
                    string userName = userDTO.Username;

                    //step 15: Set image name
                    string imageName = userId + userName + ".jpg";

                    //step 16: Set image path
                    string path = String.Format("{0}\\{1}", uploadDir, imageName);

                    //step 17: Save image
                    wi.Save(path);

                    //step 18: Login User
                    FormsAuthentication.SetAuthCookie(userModel.Username, false);
                } else {
                    return View("Index", userModel);
                }
                //step 19: redirect
                return Redirect($"~/{userModel.Username}");
            } catch (System.Data.Entity.Validation.DbEntityValidationException dbEx) {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors) {
                    foreach (var validationError in validationErrors.ValidationErrors) {
                        string message = string.Format("{0}:{1}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                    }
                }
                throw raise;
            }
        }

        //GET: /{Username}
        [Authorize]
        public ActionResult Username(string Username = "") {
            //Init DB
            lpuContext = new LpuContext();

            //check if user exists
            if (!lpuContext.Users.Any(user => user.Username.Equals(Username))) {
                return Redirect("~/");
            }
            //ViewBag Username
            ViewBag.Username = Username;

            //get logged in user's Username
            string loggedInUser = User.Identity.Name;

            //ViewBag
            User userData = lpuContext.Users.Where(user => user.Username.Equals(loggedInUser)).FirstOrDefault();
            ViewBag.FullName = userData.Firstname + " " + userData.Lastname;

            return View();
        }

        [Authorize]
        public ActionResult Logout() {
            //Signout
            FormsAuthentication.SignOut();
            return Redirect("~/");
        }

        public ActionResult LoginPartial() {
            return PartialView();
        }

        [HttpPost]
        public string Login(string Username, string Password) {
            string pass = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "SHA1");
            //Init db
            lpuContext = new LpuContext();
            //check if user exists
            if (lpuContext.Users.Any(user => user.Username.Equals(Username) && user.Password.Equals(pass))) {
                //Login user
                FormsAuthentication.SetAuthCookie(Username, false);
                return "ok";
            } else {
                return "problem";
            }
        }
    }
}