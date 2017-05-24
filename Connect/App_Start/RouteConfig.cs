using System.Web.Mvc;
using System.Web.Routing;

namespace Connect {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("AdminVerify", "Admin/Verify/{id}", new { controller = "Admin", action = "Verify", id = UrlParameter.Optional });
            routes.MapRoute("AdminIndex", "Admin/Index", new { controller = "Admin", action = "Index", id= UrlParameter.Optional });
            routes.MapRoute("AwardUpdate", "Account/{action}/{id}", new { controller = "Account", action = "AwardUpdate", id = UrlParameter.Optional });
            routes.MapRoute("SkillUpdate", "Account/{action}/{id}", new { controller = "Account", action = "SkillUpdate", id = UrlParameter.Optional });
            routes.MapRoute("ProjectUpdate", "Account/{action}/{id}", new { controller = "Account", action = "ProjectUpdate", id = UrlParameter.Optional });
            routes.MapRoute("WorkExperienceUpdate", "Account/{action}/{id}", new { controller = "Account", action = "WorkExperienceUpdate", id = UrlParameter.Optional });
            routes.MapRoute("EducationUpdate", "Account/{action}/{id}", new { controller = "Account", action = "EducationUpdate", id = UrlParameter.Optional });
            routes.MapRoute("IntroUpdate", "Account/{action}/{id}", new { controller = "Account", action = "IntroUpdate", id = UrlParameter.Optional });
            routes.MapRoute("VerifyEmail", "Account/{action}", new { controller = "Account", action = "VerifyEmail",uid=UrlParameter.Optional});
            routes.MapRoute("AwardPartial", "Account/AwardPartial", new { controller = "Account", action = "AwardPartial", awardModel = UrlParameter.Optional });
            routes.MapRoute("SkillPartial", "Account/SkillPartial", new { controller = "Account", action = "SkillPartial", skillModel = UrlParameter.Optional });
            routes.MapRoute("IntroPartial", "Account/IntroPartial", new { controller = "Account", action = "IntroPartial", IntroModel = UrlParameter.Optional });
            routes.MapRoute("ForgetPasswordPartail", "Account/ForgetPassword", new { controller = "Account", action = "ForgetPassword", IntroModel = UrlParameter.Optional });

            routes.MapRoute("ProjectPartial", "Account/ProjectPartial", new { controller = "Account", action = "ProjectPartial", projectModel = UrlParameter.Optional });
            routes.MapRoute("WorkExperiencePartial", "Account/WorkExperiencePartial", new { controller = "Account", action = "WorkExperiencePartial", eduModel = UrlParameter.Optional });
            routes.MapRoute("EducationPartial", "Account/EducationPartial", new { controller = "Account", action = "EducationPartial", eduModel = UrlParameter.Optional });
            routes.MapRoute("Profile", "Profile/{action}/{id}", new { controller = "Profile", action = "Index", id = UrlParameter.Optional });
            routes.MapRoute("Login", "Account/Login", new { controller = "Account", action = "Login" });
            routes.MapRoute("LoginPartial", "Account/LoginPartial", new { controller = "Account", action = "LoginPartial" });
            routes.MapRoute("Logout", "Account/Logout", new { controller = "Account", action = "Logout" });
            routes.MapRoute("Account", "{Username}", new { controller = "Account", action = "Username" });
            routes.MapRoute("CreateAccount", "Account/CreateAccount", new { controller = "Account", action = "CreateAccount" });
            routes.MapRoute("Branches", "Account/Branches/", new { controller = "Account", action = "Branches", courseId = UrlParameter.Optional });
            routes.MapRoute("Username", "Account/Username/", new { controller = "Account", action = "ValidateUsername", Username = UrlParameter.Optional });
            routes.MapRoute("Email", "Account/ValidateEmail/", new { controller = "Account", action = "ValidateEmail", Email = UrlParameter.Optional });
            routes.MapRoute("Default", "", new { controller = "Account", action = "Index" });
        }
    }
}