using System;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using kiosk_solution.Data.Models;

namespace kiosk_solution.Utils
{
    public class JwtUtil
    {
        public static string GenerateJSONWebToken(Party member, IConfiguration _config)
        {
            var credentials = GetCredentials(_config);
            var permClaims = new List<Claim>();
            permClaims.Add(new Claim("id", member.Id.ToString()));
            permClaims.Add(new Claim("email", member.Email));
            permClaims.Add(new Claim("role", member.Role.Name));
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              permClaims,
              expires: DateTime.Now.AddMinutes(1440),
              signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        // Validate Token 
        public static SecurityToken ValidateJSONWebToken(string token, IConfiguration _config)
        {
            var handler = new JwtSecurityTokenHandler();
            // lay securityKey từ appsetting json
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            // check token so với security
            try
            {
                ClaimsPrincipal claims = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Issuer"],
                    IssuerSigningKey = securityKey
                }, out SecurityToken validatedToken);
                return validatedToken;
            } catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }

        // Generate securityKey từ Key trong Config sau đó mã hóa ra
        public static SigningCredentials GetCredentials(IConfiguration _config)
        {

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            return new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        }

        // Generate Claim
        public static ClaimsPrincipal getClaims(string token, IConfiguration _config)
        {
            var handler = new JwtSecurityTokenHandler();
            try
            {
                // lay securityKey từ appsetting json
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                // check token
                ClaimsPrincipal claims = handler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidIssuer = _config["Jwt:Issuer"],
                    ValidAudience = _config["Jwt:Issuer"],
                    IssuerSigningKey = securityKey
                }, out SecurityToken validatedToken);
                return claims;
            }
            catch (Exception e)
            {
                throw new Exception(e.Message);
            }
        }
    }
}