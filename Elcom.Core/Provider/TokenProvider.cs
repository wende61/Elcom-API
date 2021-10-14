using System;
using System.Text;
using Elcom.Common;
using System.Security.Claims;
using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;

namespace Elcom.Core
{
    public class TokenProvider : ITokenProvider
    {
        public DycryptResponse Dycrypt(string token, string secrate)
        {
            var handler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secrate);
            var validations = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(key),
                ValidateIssuer = false,
                ValidateAudience = false
            };

            SecurityToken securityToken = null;
            try
            {
                var claims = handler.ValidateToken(token, validations, out securityToken);
                return new DycryptResponse { Claims = claims.Claims, SecurityToken = securityToken };
            }
            catch (Exception ex)
            {
                return null;
            }

           
        }
        public string Generate(DateTime expiryDate, string secrate, ClaimsIdentity claim)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secrate);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claim,
                Expires = expiryDate,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            //var value1 = Dycrypt(tokenHandler.WriteToken(token), secrate);

            return tokenHandler.WriteToken(token);
        }



        public IEnumerable<Claim> GetClaim(string token, string secrate)
        {
            var securityToken = Dycrypt(token, secrate);
            if (securityToken != null)
            {
                if (securityToken.Claims != null)
                    return securityToken.Claims;
            }
            return null;
        }

    }
}
