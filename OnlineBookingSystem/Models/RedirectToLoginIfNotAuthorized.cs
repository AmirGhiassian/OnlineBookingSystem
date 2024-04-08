using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace OnlineBookingSystem.Models
{
    public class RedirectToLoginIfNotAuthorized : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            if (context.HttpContext.User?.Identity?.IsAuthenticated != true)
            {
                //context.HttpContext.Items["Unauthorized"] = "Please log in.";
                context.Result = new RedirectToRouteResult(
                    new RouteValueDictionary
                    {
                        { "controller", "Home" },
                        { "action", "LoginPage" }
                    });
            }
            else
            {

                // Continue with the requested action
                context.Result = null;
            }
        }
    }
}
