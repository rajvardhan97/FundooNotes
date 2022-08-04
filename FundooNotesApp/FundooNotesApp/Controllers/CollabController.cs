using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using System;
using System.Linq;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CollabController : ControllerBase
    {
        private readonly ICollabBL collabBL;
        private readonly FundooContext fundooContext;
        public CollabController(ICollabBL collabBL, FundooContext fundooContext)
        {
            this.collabBL = collabBL;
            this.fundooContext = fundooContext;
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult AddCollab(CollabModel collabModel)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var collab = fundooContext.NotesTable.Where(X => X.NoteId == collabModel.NoteId).FirstOrDefault();
                if (collab.UserId == userId)
                {
                    var result = collabBL.AddCollab(collabModel);
                    if (result != null)
                    {
                        return Ok(new { Success = true, message = "Collaboration successful", data = result });
                    }
                    else
                    {
                        return BadRequest(new { Sucess = false, message = "Collaboration Failed" });
                    }
                }
                else
                {
                    return Unauthorized(new { Sucess = false, message = "Failed Collaboration" });
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("Remove")]
        public IActionResult RemoveCollab(long collabID)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "userID").Value);
                var delete = collabBL.RemoveCollab(collabID, userId);
                if (delete != null)
                {
                    return this.Ok(new { Success = true, message = "Collaboration Removed"});
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Unsuccessful" });
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
