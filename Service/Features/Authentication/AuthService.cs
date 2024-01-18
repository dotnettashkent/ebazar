/*using Microsoft.Extensions.Configuration;
using Microsoft.Toolkit.HighPerformance.Helpers;
using Service.Data;
using Shared.Features;
using Stl.Fusion.EntityFramework;
using System.Security.Claims;
using System.Text;

namespace Service.Features
{
    public class AuthService : IAuthService
    {

        private readonly IConfiguration configuration;
        private readonly IUserService userService;
        public AuthService(IConfiguration configuration, IUserService userService)
        {
            this.configuration = configuration;
            this.userService = userService;
        }

        public async ValueTask<string> GenerateTokenAsync(string login, string password)
        {
            var user = await userService.Login(login, password);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenKey = Encoding.UTF8.GetBytes(configuration["JWT:Key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim("Id", user.Id.ToString()),
                new Claim(ClaimTypes.Role, user.Role.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(10),
                SigningCredentials =
                    new SigningCredentials(new SymmetricSecurityKey(tokenKey),
                        SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);
        }
    }
}
*/