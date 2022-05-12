
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using kiosk_solution.Data.Responses;

namespace kiosk_solution.Handler
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class AuthenAtributeHandler : Attribute, IAuthorizationFilter
    {
        public string Roles;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var account = context.HttpContext.Items["User"];
            if (account != null)
            {
                string role = account?.GetType().GetProperty("role")?.GetValue(account, null).ToString();
                string[] split = Roles.Split(",");
                bool isValid = false;
                foreach(string tmp in split) {
                    if (tmp.Trim().ToLower().Equals(role.ToLower()))
                    {
                        isValid = true;
                        break;
                    }
                }
                if (!isValid)
                {
                    context.Result = new JsonResult(new ErrorResponse( 401, "You don't have permission to access"));
                }
            }
            
        }
    }
}