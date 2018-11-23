using System.Web.Mvc;

namespace UET_BTL_VERSION_1.Areas.SignIn
{
    public class SignInAreaRegistration : AreaRegistration 
    {
        public override string AreaName 
        {
            get 
            {
                return "SignIn";
            }
        }

        public override void RegisterArea(AreaRegistrationContext context) 
        {
            context.MapRoute(
                "SignIn_default",
                "SignIn/{controller}/{action}/{id}",
                new { action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}