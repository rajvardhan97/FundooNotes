using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
    public interface IUserBL
    {
        public UserEntity Registration(UserRegistrationModel userRegistrationModel);
        public string Userlogin(Login login);
        public string ForgetPassword(string email);
        public bool ResetLink(string email, string password, string confirmPassword);

    }
}
