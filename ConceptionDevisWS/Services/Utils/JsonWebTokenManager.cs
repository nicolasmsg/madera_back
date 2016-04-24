using System;
using System.Collections.Generic;
using System.IdentityModel.Protocols.WSTrust;
using System.IdentityModel.Tokens;
using System.Security.Claims;
using System.ServiceModel.Security.Tokens;
using System.Text;

namespace ConceptionDevisWS.Services.Utils
{
    /// <summary>
    /// Handle <a href="https://jwt.io/">JSON Web Token</a>
    /// </summary>
    public static class JsonWebTokenManager
    {
        private static readonly byte[] secretBytes = Encoding.UTF8.GetBytes("a3ZdUnWPo9s2_Q$E5G,2");

        /// <summary>
        /// Creates a user token
        /// </summary>
        /// <param name="userName">the user name</param>
        /// <param name="role">the requested role for this user</param>
        /// <param name="baseAddress">the host to grant rights upon</param>
        /// <returns></returns>
        public static string CreateToken(string userName, string role, string baseAddress)
        {
            
            List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Role, role)
            };
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            InMemorySymmetricSecurityKey sKey = new InMemorySymmetricSecurityKey(secretBytes);
            SecurityToken token = tokenHandler.CreateToken(makeSecurityTokenDescriptor(sKey, claims, baseAddress));
            return tokenHandler.WriteToken(token);
        }

        /// <summary>
        /// Checks a given token
        /// </summary>
        /// <param name="token">the token to check</param>
        /// <param name="baseAddress">the host to rights upon</param>
        /// <returns>the host's identity to use, if the token is valid</returns>
        /// <exception cref="ArgumentException">When the token is not a valid JSON Web Token.</exception>
        public static ClaimsPrincipal ValidateToken(string token, string baseAddress)
        {
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            TokenValidationParameters validationParams = new TokenValidationParameters
            {
                ValidAudience = baseAddress,
                ValidIssuer = "maderaConceptionDevisInc",
                ValidateIssuer = true,
                IssuerSigningToken = new BinarySecretSecurityToken(secretBytes)
            };
            SecurityToken validatedToken;
            return tokenHandler.ValidateToken(token, validationParams, out validatedToken);
        }

        private static SecurityTokenDescriptor makeSecurityTokenDescriptor(InMemorySymmetricSecurityKey sKey, List<Claim> claims, string baseAddress)
        {
            DateTime now = DateTime.UtcNow;
            Claim[] claimsArray = claims.ToArray();
            return new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                TokenIssuerName = "maderaConceptionDevisInc",
                AppliesToAddress = baseAddress,
                Lifetime = new Lifetime(now, now.AddMinutes(20)),
                SigningCredentials = new SigningCredentials(sKey,
                    "http://www.w3.org/2001/04/xmldsig-more#hmac-sha256",
                    "http://www.w3.org/2001/04/xmlenc#sha256"),
            };
        }
    }
}