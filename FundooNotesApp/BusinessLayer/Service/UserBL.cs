using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class UserBL : IUserBL
    {
        private readonly IUserRL iuserRL;

        public UserBL(IUserRL iuserRL)
        {
            this.iuserRL = iuserRL;
        }


        public UserEntity Registration(UserRegistrationModel userRegistrationModel)
        {
            try
            {
                return iuserRL.Registration(userRegistrationModel);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        public string Userlogin(Login login)
        {
            try
            {
                return iuserRL.Userlogin(login);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public string ForgetPassword(string email)
        {
            try
            {
                return iuserRL.ForgetPassword(email);
            }
            catch(Exception ex)
            {
                throw;
            }
        }
        public bool ResetLink(string email, string password, string confirmPassword)
        {
            try
            {
                return iuserRL.ResetLink(email, password, confirmPassword);
            }
            catch(Exception ex)
            {
                throw;
            }
        }

    }
}
