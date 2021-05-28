using AuthenticationAPI.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AuthenticationAPI.Repository
{
    public class LoginRepository : ILoginRepository
    {
        private readonly IConfiguration _configuration;
        private readonly UserDbContext _context;
        public LoginRepository(IConfiguration configuration, UserDbContext context)
        {
            _configuration = configuration;
            _context = context;
        }

        public UserResponse Login(UserRequest userRequest)
        {
            try
            {
                var user = _context.Users.Where(u => u.Email == userRequest.Email && u.Password == userRequest.Password).FirstOrDefault();

                if (user!=null)
                {
                    UserResponse userResponse = new UserResponse() { Id = user.UserId};
                    string token = GenerateJsonWebToken(userResponse.Id);
                    userResponse.Token = token;
                    userResponse.Message = "Login Successfull";
                    return userResponse;
                }
                return new UserResponse { Message = "Login Failed" };
            }
            catch (Exception e)
            {
                throw;
            }
        }

        private string GenerateJsonWebToken(int userId)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
            var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,userId.ToString()),
            };
            var tokenDescriptor = new JwtSecurityToken(
                issuer: _configuration["JWT:Issuer"],
                audience: _configuration["JWT:Audience"],
                expires: DateTime.Now.AddMinutes(15),
                claims: claims,
                signingCredentials: signingCredentials
                );
            string token = new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
            return token;
        }
    }
}
