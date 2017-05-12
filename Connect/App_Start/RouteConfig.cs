using System.Web.Mvc;
using System.Web.Routing;

namespace Connect {
    public class RouteConfig {
        public static void RegisterRoutes(RouteCollection routes) {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("WorkExperience", "Account/WorkExperience", new { controller = "Account", action = "WorkExperience" });
            routes.MapRoute("WorkExperiencePartial", "Account/WorkExperiencePartial", new { controller = "Account", action = "WorkExperiencePartial", eduModel = UrlParameter.Optional });
            routes.MapRoute("Education", "Account/Education", new { controller = "Account", action = "Education" });
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