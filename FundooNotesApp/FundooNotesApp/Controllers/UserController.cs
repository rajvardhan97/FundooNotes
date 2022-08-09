using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Security.Claims;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserBL iuserBL;
        private readonly ILogger<UserController> logger;

        public UserController(IUserBL iuserBL, ILogger<UserController> logger)
        {
            this.iuserBL = iuserBL;
            this.logger = logger;
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
                    logger.LogInformation("Registeration Sucessfull");
                    return Ok(new { sucess = true, message = "Registration Successfull", data = result });
                }
                else
                {
                    logger.LogError("Registeration Unsuccessfull");
                    return BadRequest(new { success = false, message = "Registration unsucessfull"});
                }
            }
            catch(System.Exception)
            {
                logger.LogError(ToString());
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
                    logger.LogInformation("Login Sucessfull");
                    return this.Ok(new { success = true, Message = "Login Successful", data = result});
                }
                else
                {
                    logger.LogError("Login Unsuccessfull");
                    return this.BadRequest(new { success = false, Message = "Login Unsuccessful"});
                }
            }
            catch (System.Exception)
            {
                logger.LogError(ToString());
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
                    logger.LogInformation("Login Sucessfull");
                    return Ok(new { success = true, Message = "Email Sent Successful" });
                }
                else
                {
                    logger.LogError("Login Unsuccessfull");
                    return BadRequest(new { success = false, Message = "Reset email not sent" });
                }
            }
            catch (System.Exception)
            {
                logger.LogError(ToString());
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
                    logger.LogInformation("Password Reset Sucessfull");
                    return Ok(new { sucess = true, Message = "Password Reset Successfully" });
                }
                else
                {
                    logger.LogError("Password Reset Unsuccessfull");
                    return BadRequest(new { success = false, Message = "Password Reset Unsuccessful" });
                }
            }
            catch (System.Exception)
            {
                logger.LogError(ToString());
                throw;
            }
        }
    }
}
