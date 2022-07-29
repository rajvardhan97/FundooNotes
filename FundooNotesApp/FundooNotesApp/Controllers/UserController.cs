using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL iuserBL;

        public UserController(IUserBL iuserBL)
        {
            this.iuserBL = iuserBL;
        }

        [HttpPost]
        [Route("Register")]
        public IActionResult RegisterUser(UserRegistrationModel userRegistrationModel)
        {
            try
            {
                var result = iuserBL.Registration(userRegistrationModel);
                if(result != null)
                {
                    return Ok(new { sucess = true, message = "Registration Successfull", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Registration unsucessfull"});
                }
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("Login")]
        public IActionResult Logins(Login login)
        {
            try
            {
                var result = iuserBL.Userlogin(login);
                if (result != null)
                {
                    return this.Ok(new { status = true, Message = "Login Successful", data = result});
                }
                else
                {
                    return this.BadRequest(new { status = false, Message = "Login Unsuccessful"});
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [HttpPost]
        [Route("ForgotPassword")]
        public IActionResult ForgotPassword(string Email)
        {
            try
            {
                var result = iuserBL.ForgetPassword(Email);
                if (result != null)
                {
                    return this.Ok(new { status = true, Message = "Email Sent Successful" });
                }
                else
                {
                    return this.BadRequest(new { status = false, Message = "Reset email not sent" });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
