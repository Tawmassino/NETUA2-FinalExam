using Microsoft.IdentityModel.Tokens;
using NETUA2_FinalExam_BackEnd.API_Services.API_Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace NETUA2_FinalExam_BackEnd.API_Services
{
    public class JwtService : IJwtService
    {
        private readonly IConfiguration _configuration;
        public JwtService(IConfiguration configuration)//get configuration from jwt information in appsetings file
        {
            _configuration = configuration;
        }

        public string GetJwtToken(string username, string role)
        {
            var claims = new List<Claim>
            {//name can be string with any name instead of "ClaimTypes.Name"
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, role)
            };

            var secretKey = _configuration.GetSection("Jwt:Key").Value;
            var issuer = _configuration.GetSection("Jwt:Issuer").Value;
            var audience = _configuration.GetSection("Jwt:Audience").Value;

            //issuing the jwt 
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));//converting utf string to byte [] 
            //jwt hash algorythm - credentials
            var cred = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);


            //generating a token
            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: cred);

            return new JwtSecurityTokenHandler().WriteToken(token);//returning a string
        }
    }
}
