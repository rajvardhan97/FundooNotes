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
                    return this.Ok(new { success = true, Message = "Login Successful", data = result});
                }
                else
                {
                    return this.BadRequest(new { success = false, Message = "Login Unsuccessful"});
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
                    return Ok(new { success = true, Message = "Email Sent Successful" });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Reset email not sent" });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }

        [Authorize]
        [HttpPost]
        [Route("ResetLink")]
        public IActionResult ResetLink(string password, string confirmPassword)
        {
            try
            {
                var Email = User.FindFirst(ClaimTypes.Email).Value.ToString();
                var result = iuserBL.ResetLink(Email, password, confirmPassword);

                if (result != null)
                {
                    return Ok(new { sucess = true, Message = "Password Reset Successfully" });
                }
                else
                {
                    return BadRequest(new { success = false, Message = "Password Reset Unsuccessful" });
                }
            }
            catch (System.Exception)
            {
                throw;
            }
        }
    }
}
