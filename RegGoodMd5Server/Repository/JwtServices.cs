using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Security;
using RegGoodMd5.Server.Models.Db_model;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace RegGoodMd5Server.Repository
{
    public class JwtServices : IJwtServices
    {
        private readonly IConfiguration _configuration;
        private readonly TimeSpan _expiry;


        public JwtServices(IConfiguration _configuration)
        {
            this._configuration = _configuration;
            var hours = _configuration.GetValue<int?>("Jwt:ExpireHours") ?? 2;
            _expiry = TimeSpan.FromHours(hours);

        }

        public string GetGenerateToken(LoginModel model)
        {
            var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]!);
            var claims = new List<Claim>
            {
                          new Claim(ClaimTypes.NameIdentifier, model.LoginID.ToString()),
                               new Claim(ClaimTypes.Name, model.userName!),
                               new Claim(ClaimTypes.Email, model.name ?? "")
            };

            var creds = new SigningCredentials
                (new SymmetricSecurityKey(key), 
                SecurityAlgorithms.HmacSha256
                );


            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.Add(_expiry),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);

        }
        public DateTime GetExipryDate() => DateTime.UtcNow.Add(_expiry);
    }
}
