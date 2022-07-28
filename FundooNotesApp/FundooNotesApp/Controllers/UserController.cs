using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        
    }
}
