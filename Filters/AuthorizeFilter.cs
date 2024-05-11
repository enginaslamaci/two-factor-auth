using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace LoginWith2FA.Filters
{
    public class AuthorizeFilter : ActionFilterAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var loginResult = context.HttpContext.Session.GetString("LoginResult");
            if (loginResult != "success")
                context.Result = new RedirectResult("/Auth/Login");
        }
    }
}
