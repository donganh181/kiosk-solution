using kiosk_solution.Business.Utilities;
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace kiosk_solution.Utils
{
    public class HttpContextUtil
    {
        public static string getRoleFromContext(HttpContext context)
        {
            var account = context.Items["User"];
            string role = account?.GetType().GetProperty("role")?.GetValue(account, null).ToString();
            return role;
        }
        public static string getRoleFromRequest(HttpRequest request,IConfiguration configuration)
        {
            if (request.Headers[AuthenConstant.HEADER_AUTHEN_KEY].Count > 0)
            {
                var token = request.Headers[AuthenConstant.HEADER_AUTHEN_KEY];
                TokenViewModel tokenModel = TokenUtil.ReadJWTTokenToModel(token, configuration);
                return tokenModel.Role;
            }
            return null;
        }
        public static TokenViewModel getTokenModelFromRequest(HttpRequest request,IConfiguration configuration)
        {
            if (request.Headers[AuthenConstant.HEADER_AUTHEN_KEY].Count > 0)
            {
                var token = request.Headers[AuthenConstant.HEADER_AUTHEN_KEY];
                return TokenUtil.ReadJWTTokenToModel(token, configuration);
            }
            return null;
        }
    }
}