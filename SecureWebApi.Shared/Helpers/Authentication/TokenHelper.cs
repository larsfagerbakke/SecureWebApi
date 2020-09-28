using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using SecureWebApi.Shared.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace SecureWebApi.Shared.Helpers.Authentication
{
    public class TokenHelper
    {
        public static string CreateToken(UserModel user, string jwtKey, string jwtIssuer, string jwtAudience)
        {
            if (user == null)
                throw new ArgumentException(nameof(user));

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            foreach (var r in user.Roles)
                claims.Add(new Claim(ClaimTypes.Role, r.ToString()));

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var token = new JwtSecurityToken(jwtIssuer,
                jwtAudience,
                claims,
                expires: DateTime.Now.AddMonths(3),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public static UserModel ReadToken(string tokenString, string jwtKey, string jwtIssuer, string jwtAudience)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

            var handler = new JwtSecurityTokenHandler();

            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ValidateIssuer = true,
                ValidIssuer = jwtIssuer,
                ValidateAudience = true,
                ValidAudience = jwtAudience
            };

            var identity = handler.ValidateToken(tokenString, validations, out var tokenSecure).Identity as ClaimsIdentity;

            if (identity == null)
            {
                throw new Exception("Invalid identity");
            }

            var user = new UserModel
            {
                Id = Guid.Parse(identity.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value),
            };

            return user;
        }
    }
}
