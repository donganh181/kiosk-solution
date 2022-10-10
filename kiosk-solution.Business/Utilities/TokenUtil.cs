
using kiosk_solution.Data.Constants;
using kiosk_solution.Data.Models;
using kiosk_solution.Data.Responses;
using kiosk_solution.Data.ViewModels;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace kiosk_solution.Business.Utilities
{
    public static class TokenUtil
    {
        private static string secretKey;
        private static void setPrivateKey(IConfiguration configuration)
        {
            secretKey = configuration.GetSection("Security:SecretKey").Value;
        }

        public static string GenerateJWTWebToken(PartyViewModel partyInfo, IConfiguration configuration)
        {
            setPrivateKey(configuration);

            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
                        new Claim(PayloadKeyConstants.ID, partyInfo.Id.ToString()),
                        new Claim(PayloadKeyConstants.ROLE, partyInfo.RoleName),
                        new Claim(PayloadKeyConstants.MAIL, partyInfo.Email),
                        new Claim(PayloadKeyConstants.PHONE_NUMBER, partyInfo.PhoneNumber)
            };

            var token = new JwtSecurityToken("",
                "",
                claims,
                expires: DateTime.Now.AddMonths(2),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static TokenViewModel ReadJWTTokenToModel(string token, IConfiguration configuration)
        {
            string tempToken = token;

            string role = null;
            string mail = null;
            string phoneNum = null;
            

            if (token.Contains("Bearer"))
            {
                token = tempToken.Split(' ')[1];
            }

            setPrivateKey(configuration);

            bool isValid = IsTokenValid(token);

            if (!isValid)
            {
                throw new ErrorResponse((int)HttpStatusCode.Unauthorized, "Request Secret Token is invalid");
            }

            var result = new JwtSecurityTokenHandler().ReadJwtToken(token);
            Guid id = Guid.Parse(result.Claims.First(claim => claim.Type == PayloadKeyConstants.ID).Value);
            role = result.Claims.First(claim => claim.Type == PayloadKeyConstants.ROLE).Value;
            mail = result.Claims.First(claim => claim.Type == PayloadKeyConstants.MAIL).Value;
            phoneNum = result.Claims.First(claim => claim.Type == PayloadKeyConstants.PHONE_NUMBER).Value;
            return new TokenViewModel(id, role, mail, phoneNum);
        }

        private static SecurityKey GetSymmetricSecurityKey()
        {
            byte[] symmetricKey = Convert.FromBase64String(secretKey);
            return new SymmetricSecurityKey(symmetricKey);
        }

        private static TokenValidationParameters GetTokenValidationParameters()
        {
            return new TokenValidationParameters()
            {
                ValidateIssuer = false,
                ValidateAudience = false,
                IssuerSigningKey = GetSymmetricSecurityKey(),
                ValidateLifetime = false
            };
        }

        private static bool IsTokenValid(string token)
        {
            try
            {
                ClaimsPrincipal tokenValid = new JwtSecurityTokenHandler().
                    ValidateToken(token, GetTokenValidationParameters(), out SecurityToken validatedToken);
                return true;
            }
            catch
            {
                return false;
            }
        }
        public static SecurityToken ValidateJSONWebToken(string token, IConfiguration _config)
        {
            var handler = new JwtSecurityTokenHandler();
            // lay securityKey từ appsetting json
            var securityKey = new SymmetricSecurityKey(Convert.FromBase64String(secretKey));
            // check token so với security
            try
            {
                ClaimsPrincipal claims = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = false,
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    IssuerSigningKey = securityKey
                }, out SecurityToken validatedToken);
                return validatedToken;
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}
