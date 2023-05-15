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
        string BuildToken(UserModelDTO? um, IConfiguration conf);
    }

    public class TokenMng : ITokenMng
    {

        public TokenMng()
        {
        }

        public string BuildToken(UserModelDTO? um, IConfiguration conf)
        {
            List<Claim> llClaim = new List<Claim>();
            llClaim.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Name, um.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(nameof(um.UserName), um.UserName),
                new Claim(nameof(um.Id), um.Id.ToString()),
                new Claim(ClaimTypes.Role, um.UserType.ToString())
            });


            switch (um.UserType)
            {
                case UserModelDTO.UserModelType.ADMIN:                    
                    break;

                case UserModelDTO.UserModelType.EMPLOYEE:
                    llClaim.AddRange(new[]
                    { 
                        //new Claim(JwtRegisteredClaimNames.Birthdate, um.Birthdate.ToString("yyyy-MM-dd")),
                        new Claim(um.JobQualification, "true"),
                        new Claim(nameof(um.JobQualification), um.JobQualification)
                    });                    

                    break;

                case UserModelDTO.UserModelType.CUSTOMER:
                    break;
                default:
                    break;
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(conf["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                conf["Jwt:Issuer"],
                conf["Jwt:Issuer"],
                llClaim,
                expires: DateTime.Now.AddDays(30), //expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            string tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return tokenString;
        }


    }
}
