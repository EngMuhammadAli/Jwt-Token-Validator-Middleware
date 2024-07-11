using Jwt_Token_Validator_Middleware.Data;
using Jwt_Token_Validator_Middleware.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Jwt_Token_Validator_Middleware.TokenServices
{
    public class Token : IToken
    {
        private readonly IConfiguration _configuration;
        private readonly ApplicationDbContext _context;

        public Token(IConfiguration configuration, ApplicationDbContext applicationDbContext)
        {
            _configuration = configuration;
            _context = applicationDbContext;
        }
        public string GenerateJWT(User user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, GetUserRole(user.UserID)) // Add user role to claims
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(_configuration["Jwt:ExpiryInMinutes"])),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string GetUserRole(int userId)
        {
            int roleID = 0;
            User userRole = _context.Users.FirstOrDefault(u => u.UserID == userId);
            if (userRole != null)
            {
                roleID = userRole.RoleID;
                var role = _context.Roles.FirstOrDefault(r => r.RoleID == roleID);
                if (role != null)
                {
                    return role.RoleName;
                }
            }
            return "Player"; // Default role if none found
        }
    }
}
