using Connect.Models;
using Connect.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Security;

namespace Connect.Controllers {
    public class AccountController : Controller {
        LpuContext lpuContext = new LpuContext();
        Record recordDTO;
        // GET: /

        #region username and email validation

        public JsonResult ValidateUsername(string Username) {
            return Json(!lpuContext.Users.Any(user => user.Username.Equals(Username)), JsonRequestBehavior.AllowGet);
        }
        public JsonResult ValidateEmail(string Email) {
            return Json(!lpuContext.Users.Any(user => user.Email.Equals(Email)), JsonRequestBehavior.AllowGet);
        }
        #endregion

        public ActionResult Index() {
            //Fetching and adding course and department in the viewbag
            ViewBag.Department = lpuContext.Departments;
            ViewBag.Courses = lpuContext.Courses;

            //confirm user not logged in 
            string Username = User.Identity.Name;
            if (!String.IsNullOrEmpty(Username)) {
                return Redirect($"~/{Username}");
            } else {
                lpuContext = new LpuContext();
                List<Course> courses = lpuContext.Courses.ToList();
                ViewBag.Course = courses;
                //return view
                return View();
            }
        }

        #region Fill Branch
        public ActionResult Branches(int courseId) {
            lpuContext.Configuration.ProxyCreationEnabled = false;
            var branches = lpuContext.Branches.Where(x => x.CourseId == courseId);
            return Json(branches);
        }
        #endregion

        //POST: Account/CreateAccount
        [HttpPost]
        public ActionResult CreateAccount(FormCollection role, RegisterVM userModel, HttpPostedFileBase file) {
            try {
                //Step 1: Get Role
                string UserRole = role["Role"].ToString();

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
                } else if(userModel.Department == null){
                    ModelState.AddModelError("", "Department required");
                    return View("Index", userModel);
                }
                //step 5: Create UserDTO object
                User userDTO = new User() {
                    Firstname = userModel.Firstname,
                    Lastname = userModel.Lastname,
                    Username = userModel.Username,
                    Email = userModel.Email,
                    City = userModel.City,
                    Password = FormsAuthentication.HashPasswordForStoringInConfigFile(userModel.Password, "SHA1")
                };
                if (UserRole == "1") {
                    recordDTO = new Record() {
                        LpuId = userModel.LpuId,
                        Department = userModel.Department,
                        Title = userModel.Title,
                        FromYear = userModel.FromYear,
                        ToYear = userModel.ToYear,
                        CurrentlyWorking = userModel.CurrentlyWorking,
                        UserId = userDTO.UserId
                    };
                } else if (UserRole == "2") {
                    recordDTO = new Record() {
                        LpuId = userModel.LpuId,
                        Course = userModel.Course,
                        Branch = userModel.Branch,
                        EntryYear = userModel.EntryYear,
                        GraduateYear = userModel.GraduateYear,
                        UserId = userDTO.UserId
                    };
                }
                //step 6: Add basic user data
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
                    lpuContext.Records.Add(recordDTO);
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