using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace RepositoryLayer.Service
{
    public class UserRL : IUserRL
    {
        private readonly IConfiguration configuration;
        private readonly FundooContext fundooContext;

        public UserRL(FundooContext fundooContext, IConfiguration configuration)
        {
            this.fundooContext = fundooContext;
            this.configuration = configuration;
        }

        public UserEntity Registration(UserRegistrationModel userRegistrationModel)
        {
            try
            {
                UserEntity userEntity = new UserEntity();
                userEntity.FirstName = userRegistrationModel.FirstName;
                userEntity.LastName = userRegistrationModel.LastName;
                userEntity.Email = userRegistrationModel.Email;
                userEntity.Password = ConvertToEncrypt(userRegistrationModel.Password);

                fundooContext.UserTable.Add(userEntity);
                int result = fundooContext.SaveChanges();

                if (result != 0)
                {
                    return userEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string Userlogin(Login login)
        {
            try
            {
                var result = fundooContext.UserTable.Where(a => a.Email == login.Email && a.Password == ConvertToDecrypt(login.Password)).FirstOrDefault();
                if (result != null)
                {
                    var token = GenerateSecurityToken(result.Email, result.UserId);
                    return token;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string GenerateSecurityToken(string email, long userID)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(configuration[("JWT:key")]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.Email, email),
                    new Claim("userID", userID.ToString())
                }),
                Expires = DateTime.UtcNow.AddMinutes(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return tokenHandler.WriteToken(token);

        }

        public string ForgetPassword(string email)
        {
            try
            {
                var EmailCheck = fundooContext.UserTable.FirstOrDefault(x => x.Email == email);
                if(EmailCheck != null)
                {
                    var Token = GenerateSecurityToken(EmailCheck.Email, EmailCheck.UserId);
                    MSMQmodel mSMQmodel = new MSMQmodel();
                    mSMQmodel.sendData2Queue(Token);
                    return Token;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool ResetLink(string email, string password, string confirmPassword)
        {
            try
            {
                if(password.Equals(confirmPassword))
                {
                    var EmailCheck = fundooContext.UserTable.FirstOrDefault(x => x.Email == email);
                    EmailCheck.Password = password;

                    fundooContext.SaveChanges();
                    return true;
                }
                else
                {
                    return false;
                }

            }
            catch (Exception)
            {
                throw;
            }

        }

        string Key = "adef@kfxcbv@";
        public string ConvertToEncrypt(string password)
        {
            if (string.IsNullOrEmpty(password)) return "";
            password += Key;
            var passwordBytes = Encoding.UTF8.GetBytes(password);
            return Convert.ToBase64String(passwordBytes);
        }

        public string ConvertToDecrypt(string base64EncodeData)
        {
            if (string.IsNullOrEmpty(base64EncodeData)) return "";
            var base64EncodeBytes = Convert.FromBase64String(base64EncodeData);
            var result = Encoding.UTF8.GetString(base64EncodeBytes);
            result = result.Substring(0, result.Length - Key.Length);
            return result;
        }
    }
}
