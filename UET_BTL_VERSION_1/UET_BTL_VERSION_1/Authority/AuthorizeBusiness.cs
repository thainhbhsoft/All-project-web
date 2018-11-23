using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using UET_BTL.Model.Entities;

namespace UET_BTL.Model.Authority
{
    public class AuthorizeBusiness : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            base.OnActionExecuting(filterContext);
            User user = HttpContext.Current.Session["user"] as User;
            if (user == null)
            {
                filterContext.Result = new RedirectResult("/SignIn/Home/Login");
                return;
            }
            var dataTokens = HttpContext.Current.Request.RequestContext.RouteData.DataTokens;
            var routeValues = HttpContext.Current.Request.RequestContext.RouteData.Values;
            string action = routeValues.ContainsKey("action") ? (string)routeValues["action"] : string.Empty;
            string controller = routeValues.ContainsKey("controller") ? (string)routeValues["controller"] : string.Empty;
            string area = dataTokens.ContainsKey("area") ? (string)dataTokens["area"] : string.Empty;
            
            UetSurveyDbContext db = new UetSurveyDbContext();
            bool check = db.Roles.Any(s => s.Position == user.Position && s.Area == area  && s.Controller == controller && s.Action == action);

            if (!check)
            {
                filterContext.Result = new RedirectResult("~/SignIn/Home/Authorize");
                return;
            }
        }

    }
}
