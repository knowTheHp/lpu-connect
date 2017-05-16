using Connect.Models;
using Connect.Models.Repository;
using Connect.Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
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
                } else if (userModel.Department == null) {
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
                    IsUserVerified = false,
                    RegisteredDateTime = DateTime.Now,
                    registeredUniqueId = Guid.NewGuid(),
                    IsEmailVerified = false,
                    Password = FormsAuthentication.HashPasswordForStoringInConfigFile(userModel.Password, "SHA1")
                };
                VerifyEmail emailVerification = new VerifyEmail() {
                    UniqueId = Guid.NewGuid(),
                    VerificationDateTime = DateTime.Now,
                    UserId = userDTO.UserId
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
                        Title = "Student",
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
                    lpuContext.Records1.Add(recordDTO);
                    lpuContext.VerifyEmails.Add(emailVerification);
                    lpuContext.SaveChanges();

                    EmailVerification.SendEmail(userDTO.UserId, userDTO.Email, userDTO.Firstname + ' ' + userDTO.Lastname, emailVerification.UniqueId.ToString());

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

        #region Email Verification
        [HttpPost]
        [AllowAnonymous]
        public ActionResult VerifyEmail(string userId, string uid) {
            string uniqueId = Request.QueryString["uid"];
            if (!string.IsNullOrEmpty(uid)) {
                VerifyEmail email = lpuContext.VerifyEmails.Where(uidCode => uidCode.UniqueId.ToString() == uniqueId).FirstOrDefault();
                if (email.UniqueId != null) {
                    User userEmail = lpuContext.Users.Where(user => user.UserId == Convert.ToInt64(userId)).FirstOrDefault();
                    userEmail.IsEmailVerified = true;
                    lpuContext.Users.Add(userEmail);
                    lpuContext.VerifyEmails.Remove(email);
                    lpuContext.SaveChanges();
                    ViewBag.ValidateEmail = "Email verified";
                } else {
                    ViewBag.ValidateEmail = "Verification link expired";
                }
            }
            return View("VerifyEmail");
        }
        #endregion

        //GET: /{Username}
        [Authorize]
        public ActionResult Username(string Username = "") {
            //check if user exists
            if (!lpuContext.Users.Any(user => user.Username.Equals(Username))) {
                return Redirect("~/");
            }
            //ViewBag Username
            ViewBag.Username = Username;
            //get logged in user's Username
            string loggedInUser = User.Identity.Name;

            #region Logged in User Data
            User userLoggedIn = lpuContext.Users.Where(user => user.Username.Equals(loggedInUser)).FirstOrDefault();
            //get user fullname
            ViewBag.FullName = userLoggedIn.Firstname + " " + userLoggedIn.Lastname;
            //get user id
            long userId = userLoggedIn.UserId;
            ViewBag.UserId = userId;
            #endregion

            #region Get Viewing Data
            Record recordView = lpuContext.Records1.Where(user => user.Users.Username.Equals(Username)).FirstOrDefault();
            //get fullname
            ViewBag.FullName = recordView.Users.Firstname + " " + recordView.Users.Lastname;
            //get image
            ViewBag.UserImage = recordView.UserId + recordView.Users.Username + ".jpg";
            //get city
            ViewBag.City = recordView.Users.City;
            //get role
            ViewBag.Role = recordView.Title;
            //get email
            ViewBag.Email = recordView.Users.Email;
            //get department
            if (recordView.Department != null) {
                ViewBag.Department = recordView.Departments.DepartmentName;
            }
            //get course
            if (recordView.Course != null) {
                ViewBag.Course = recordView.Courses.CourseName;
            }
            //get branch
            if (recordView.Branch != null) {
                ViewBag.Branch = recordView.Branches.BranchName;
            }
            //get from year for employees
            if (recordView.FromYear != null) {
                ViewBag.From = recordView.FromYear;
            }
            //get from year for employees
            if (recordView.ToYear != null) {
                ViewBag.ToYear = recordView.ToYear;
            }
            //get entry year
            if (recordView.EntryYear != null) {
                ViewBag.EntryYear = recordView.EntryYear;
            }
            //get graduate year
            if (recordView.GraduateYear != null) {
                ViewBag.GraduateYear = recordView.GraduateYear;
            }
            //get intro
            var intro = recordView.Users.Intro.Where(user => user.User.Username == Username).FirstOrDefault();
            if (intro != null) {
                ViewBag.Intro = intro.Intro;
                ViewBag.IntroId = intro.IntroId;
            } else {
                ViewBag.intro = null;
            }

            //get education list record
            ViewBag.Education = recordView.Users.Educations.Where(user => user.User.Username == Username).ToList();

            //get workXp
            ViewBag.WorkXp = recordView.Users.WorkXp.Where(user => user.User.Username == Username).ToList();

            //get Project
            ViewBag.Projects = recordView.Users.Projects.Where(user => user.User.Username == Username).ToList();

            //get awards
            ViewBag.Awards = recordView.Users.Awards.Where(user => user.User.Username == Username).ToList();

            //get skills
            ViewBag.Skills = recordView.Users.Skills.Where(user => user.User.Username == Username).ToList();
            #endregion

            //check the usertype
            string userType = "guest";
            if (Username.Equals(loggedInUser)) {
                userType = "owner";
            }

            //viewbag usertype
            ViewBag.UserType = userType;

            //check if they are friends
            if (userType == "guest") {
                User self = lpuContext.Users.Where(x => x.Username.Equals(loggedInUser)).FirstOrDefault();
                long selfId = self.UserId;

                User friend = lpuContext.Users.Where(x => x.Username.Equals(Username)).FirstOrDefault();
                long friendId = friend.UserId;

                Connection fromConnection = lpuContext.Connections.Where(user => user.User_Sender == selfId && user.User_Receiver == friendId).FirstOrDefault();
                Connection toConnection = lpuContext.Connections.Where(user => user.User_Receiver == selfId && user.User_Sender == friendId).FirstOrDefault();

                if (fromConnection == null && toConnection == null) {
                    ViewBag.Friends = "True";
                }
                if (toConnection != null) {
                    if (!toConnection.Active) {
                        ViewBag.Friends = "Pending";
                    }
                }

                if (fromConnection != null) {
                    if (!fromConnection.Active) {
                        ViewBag.Friends = "Pending";
                    }
                }
            }

            //get connection request count
            int requestCount = lpuContext.Connections.Count(x => x.User_Receiver == userId && x.Active == false);
            if (requestCount > 0) {
                ViewBag.Requests = requestCount;
            }

            //get connected count
            User userViewedData = lpuContext.Users.Where(user => user.Username.Equals(Username)).FirstOrDefault();
            long userViewedId = userViewedData.UserId;

            var connectionCount = lpuContext.Connections.Count(connection => connection.User_Receiver == userViewedId && connection.Active == true || connection.User_Sender == userViewedId && connection.Active == true);
            ViewBag.Connection = connectionCount;

            return View();
        }

        #region Login View
        public ActionResult LoginPartial() {
            return PartialView();
        }
        #endregion

        #region Login Validation
        [HttpPost]
        public string Login(string Username, string Password) {
            string pass = FormsAuthentication.HashPasswordForStoringInConfigFile(Password, "SHA1");
            //check if user exists
            if (lpuContext.Users.Any(user => user.Username.Equals(Username) && user.Password.Equals(pass))) {
                //Login user
                FormsAuthentication.SetAuthCookie(Username, false);
                return "ok";
            } else {
                return "problem";
            }
        }
        #endregion

        #region IntroPartial
        public ActionResult IntroPartial() {
            SelfIntroVM intro = new SelfIntroVM();
            return PartialView("IntroPartial", intro);
        }
        #endregion

        #region Intro Validation and Insertion
        public ActionResult Intro(SelfIntroVM introVM) {
            SelfIntro selfIntro = new SelfIntro() {
                Intro = introVM.Intro,
                UserId = lpuContext.Users.Where(user => user.Username.Equals(User.Identity.Name)).Single().UserId
            };
            lpuContext.SelfIntroes.Add(selfIntro);
            lpuContext.SaveChanges();
            return Redirect("~/");
        }
        #endregion

        #region Update Intro View
        [HttpGet]
        [Authorize]
        public ActionResult IntroUpdate(long? id) {
            SelfIntro selfIntro = lpuContext.SelfIntroes.Single(intro => intro.IntroId == id);
            return View("IntroUpdate", selfIntro);
        }
        #endregion

        #region Update Intro Data
        [HttpPost]
        [Authorize]
        public ActionResult IntroUpdate(SelfIntro entity) {
            lpuContext.Configuration.ProxyCreationEnabled = false;
            lpuContext.Entry(entity).State = EntityState.Modified;
            lpuContext.SaveChanges();
            return Redirect("~/");
        }
        #endregion

        #region Education View
        [HttpGet]
        [Authorize]
        public ActionResult EducationPartial() {
            EducationVM eduModel = new EducationVM();
            ViewBag.Degree = lpuContext.Degrees;
            ViewBag.FromYear = lpuContext.Years1.OrderByDescending(x => x.YearId);
            ViewBag.ToYear = lpuContext.Years1.OrderByDescending(x => x.YearId);
            return PartialView("EducationPartial", eduModel);
        }
        #endregion

        #region Education Validation and Insertion
        [HttpPost]
        [Authorize]
        public ActionResult Education(EducationVM model) {
            if (ModelState.IsValid) {
                Education educationDTO = new Education() {
                    School = model.School,
                    DegreeType = model.DegreeType,
                    Course = model.Course,
                    EduFrom = model.EduFrom,
                    EduTo = model.EduTo,
                    UserId = lpuContext.Users.Where(user => user.Username.Equals(User.Identity.Name)).Single().UserId
                };
                lpuContext.Educations.Add(educationDTO);
                lpuContext.SaveChanges();
                return Redirect("~/");
            } else {
                return View("~/");
            }
        }
        #endregion

        #region Update Education View
        [HttpGet]
        [Authorize]
        public ActionResult EducationUpdate(long? id) {
            ViewBag.Degree = lpuContext.Degrees;
            ViewBag.FromYear = lpuContext.Years1.OrderByDescending(x => x.YearId);
            ViewBag.ToYear = lpuContext.Years1.OrderByDescending(x => x.YearId);
            Education eduData = lpuContext.Educations.Where(edu => edu.EducationId == id).Single();
            return View("EducationUpdate", eduData);

        }
        #endregion

        #region Update Intro Data
        [HttpPost]
        [Authorize]
        public ActionResult EducationUpdate(Education entity) {
            lpuContext.Entry(entity).State = EntityState.Modified;
            lpuContext.SaveChanges();
            return Redirect("~/");
        }
        #endregion

        #region WorkXp View
        [HttpGet]
        [Authorize]
        public ActionResult WorkExperiencePartial() {
            WorkXpVM workXpVM = new WorkXpVM();
            ViewBag.Countries = lpuContext.Countries;
            var Months = lpuContext.Months;
            var Years = lpuContext.Years1.OrderByDescending(x => x.YearId);
            ViewBag.FromMonth = Months;
            ViewBag.FromYear = Years;
            ViewBag.ToMonth = Months;
            ViewBag.ToYear = Years;
            return View("WorkExperiencePartial", workXpVM);
        }
        #endregion


        #region WorkXp Validation and Insertion
        [Authorize]
        [HttpPost]
        public ActionResult WorkExperience(WorkXpVM workXp) {
            WorkXp workXpDTO = new WorkXp() {
                Company = workXp.Company,
                Designation = workXp.Designation,
                City = workXp.City,
                Country = workXp.Country,
                FromMonth = workXp.FromMonth,
                FromYear = workXp.FromYear,
                ToMonth = workXp.ToMonth,
                ToYear = workXp.ToYear,
                IsCurrentlyWorking = workXp.IsCurrentlyWorking,
                UserId = lpuContext.Users.Where(user => user.Username.Equals(User.Identity.Name)).Single().UserId
            };
            lpuContext.WorkXps.Add(workXpDTO);
            lpuContext.SaveChanges();
            return Redirect("~/");
        }
        #endregion

        #region WorkXpUpdate View
        [HttpGet]
        [Authorize]
        public ActionResult WorkExperienceUpdate(long? id) {
            WorkXp workXpData = lpuContext.WorkXps.Where(workXp => workXp.WorkxpId == id).Single();
            ViewBag.Countries = lpuContext.Countries;
            var Months = lpuContext.Months;
            var Years = lpuContext.Years1.OrderByDescending(x => x.YearId);
            ViewBag.ToYear = Years;
            ViewBag.FromMonth = Months;
            ViewBag.FromYear = Years;
            ViewBag.ToMonth = Months;
            return View("WorkExperienceUpdate", workXpData);
        }
        #endregion

        #region Update WorkXp Data
        [HttpPost]
        [Authorize]
        public ActionResult WorkExperienceUpdate(WorkXp entity) {
            lpuContext.Entry(entity).State = EntityState.Modified;
            lpuContext.SaveChanges();
            return Redirect("~/");
        }
        #endregion

        #region Project View
        public ActionResult ProjectPartial() {
            ProjectVM projectVM = new ProjectVM();
            var Months = lpuContext.Months;
            var Years = lpuContext.Years1.OrderByDescending(x => x.YearId);
            ViewBag.FromMonth = Months;
            ViewBag.FromYear = Years;
            ViewBag.ToMonth = Months;
            ViewBag.ToYear = Years;
            return View("ProjectPartial", projectVM);
        }
        #endregion

        #region Project Validation and Insertion
        public ActionResult Project(ProjectVM projectVM) {
            Project project = new Project() {
                ProjectName = projectVM.ProjectName,
                ProjectDescription = projectVM.ProjectDescription,
                ProjectUrl = projectVM.ProjectUrl,
                ProjectStartMonth = projectVM.ProjectStartMonth,
                ProjectStartYear = projectVM.ProjectStartYear,
                ProjectOnGoing = projectVM.ProjectOnGoing,
                ProjectEndMonth = projectVM.ProjectEndMonth,
                ProjectEndYear = projectVM.ProjectEndYear,
                UserId = lpuContext.Users.Where(user => user.Username.Equals(User.Identity.Name)).Single().UserId
            };
            lpuContext.Projects.Add(project);
            lpuContext.SaveChanges();
            return Redirect("~/");
        }
        #endregion

        #region ProjectUpdate View
        [HttpGet]
        [Authorize]
        public ActionResult ProjectUpdate(long? id) {
            ViewBag.Countries = lpuContext.Countries;
            ViewBag.FromMonth = lpuContext.Months;
            ViewBag.FromYear = lpuContext.Years1.OrderByDescending(fromYear => fromYear.YearId);
            ViewBag.ToMonth = lpuContext.Months;
            ViewBag.ToYear = lpuContext.Years1.OrderByDescending(toYear => toYear.YearId);
            ViewBag.Degree = lpuContext.Degrees;
            Project projectData = lpuContext.Projects.Where(project => project.ProjectId== id).Single();
            return View("ProjectUpdate", projectData);
        }
        #endregion

        #region Update Project Data
        [HttpPost]
        [Authorize]
        public ActionResult ProjectUpdate(Project entity) {
            lpuContext.Entry(entity).State = EntityState.Modified;
            lpuContext.SaveChanges();
            return Redirect("~/");
        }
        #endregion


        #region Skills View
        public ActionResult SkillPartial() {
            SkillsVM skillsVM = new SkillsVM();
            ViewBag.Skills = lpuContext.Skills;
            return PartialView("SkillPartial", skillsVM);
        }
        #endregion

        #region Skills Validation and Insertion
        public ActionResult Skill(SkillsVM skillsVM) {
            UserSkills userSkills = new UserSkills() {
                SkillId = skillsVM.SkillId,
                UserId = lpuContext.Users.Where(user => user.Username.Equals(User.Identity.Name)).Single().UserId
            };
            lpuContext.UserSkills.Add(userSkills);
            lpuContext.SaveChanges();
            return Redirect("~/");
        }
        #endregion

        #region User Skills Update View
        [HttpGet]
        [Authorize]
        public ActionResult SkillUpdate(long? id) {
            ViewBag.Skills = lpuContext.Skills;
            UserSkills skillData = lpuContext.UserSkills.Where(skill => skill.UserSkillId == id).Single();
            return View("SkillUpdate", skillData);
        }
        #endregion

        #region Update User Skills
        [HttpPost]
        [Authorize]
        public ActionResult SkillUpdate(UserSkills entity) {
            lpuContext.Entry(entity).State = EntityState.Modified;
            lpuContext.SaveChanges();
            return Redirect("~/");
        }
        #endregion


        #region Awards View
        [HttpGet]
        [Authorize]
        public ActionResult AwardPartial() {
            AwardVM awardVM = new AwardVM();
            ViewBag.Month = lpuContext.Months;
            ViewBag.Year = lpuContext.Years1.OrderByDescending(x => x.YearId);
            return PartialView("AwardPartial", awardVM);
        }
        #endregion

        [HttpPost]
        [Authorize]
        #region Awards Validation and Insertion
        public ActionResult Award(AwardVM awardVM) {
            Award award = new Award() {
                Name = awardVM.Name,
                Issuer = awardVM.Issuer,
                Description = awardVM.Description,
                AwardMonth = awardVM.AwardMonth,
                AwardYear = awardVM.AwardYear,
                UserId = lpuContext.Users.Where(user => user.Username.Equals(User.Identity.Name)).Single().UserId
            };
            lpuContext.Awards.Add(award);
            lpuContext.SaveChanges();
            return Redirect("~/");
        }
        #endregion

        #region Award Update View
        [HttpGet]
        [Authorize]
        public ActionResult AwardUpdate(long? id) {
            ViewBag.Month = lpuContext.Months;
            ViewBag.Year = lpuContext.Years1.OrderByDescending(x => x.YearId);
            Award userAwards = lpuContext.Awards.Where(award => award.AwardId == id).Single();
            return View("AwardUpdate", userAwards);
        }
        #endregion

        #region Update User Award
        [HttpPost]
        [Authorize]
        public ActionResult AwardUpdate(Award entity) {
            lpuContext.Entry(entity).State = EntityState.Modified;
            lpuContext.SaveChanges();
            return Redirect("~/");
        }
        #endregion


        #region Logout
        [Authorize]
        public ActionResult Logout() {
            //Signout
            FormsAuthentication.SignOut();
            return Redirect("~/");
        }
        #endregion
    }
}