using CinemaDTO;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace CinemaBL
{
    public interface ITokenMng
    {
        object BuildToken(UserModel? um, IConfiguration conf);
        string GetValToken();
    }

    public class TokenMng : ITokenMng
    {

        public TokenMng()
        {
        }

        public object BuildToken(UserModel? um, IConfiguration conf)
        {
            List<Claim> llClaim = new List<Claim>();
            llClaim.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, um.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, um.UserType.ToString())
            });


            switch (um.UserType)
            {
                case UserModel.UserModelType.ADMIN:
                    break;

                case UserModel.UserModelType.EMPLOYEE:
                    llClaim.Add(new Claim(JwtRegisteredClaimNames.Birthdate, um.Birthdate.ToString("yyyy-MM-dd")));
                    llClaim.Add(new Claim(ClaimTypes.Role, um.JobQualification));

                    break;

                case UserModel.UserModelType.CUSTOMER:
                    break;
                default:
                    break;
            }

            //var claims = new[] {
            //    new Claim(JwtRegisteredClaimNames.Name,  um.Name),
            //    //new Claim(JwtRegisteredClaimNames.Email, user.Email),
            //    //new Claim(JwtRegisteredClaimNames.Birthdate, um.Birthdate.ToString("yyyy-MM-dd")),
            //    //new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //    new Claim(ClaimTypes.Role, "Administrator") //role base auth
            //    //new Claim(ClaimTypes.Role, um.JobQualification)
            //};
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                conf["Jwt:Issuer"],
                conf["Jwt:Issuer"],
                llClaim,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);


            //var token = new JwtSecurityToken(
            //    conf["Jwt:Issuer"],
            //    conf["Jwt:Issuer"],
            //    claims,
            //    expires: DateTime.Now.AddMinutes(30),
            //    signingCredentials: creds);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }

        public string GetValToken()
        {
            return "tooooken";
        }


    }
}
