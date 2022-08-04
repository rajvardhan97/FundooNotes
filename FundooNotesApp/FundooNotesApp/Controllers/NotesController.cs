using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INotesBL notesBL;
        public NotesController(INotesBL notesBL)
        {
            this.notesBL = notesBL;
        }
        [HttpPost]
        [Route("Create")]
        public IActionResult CreateNotes(NotesModel notesModel)
        {
            try
            {

                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(e => e.Type == "userID").Value);
                var result = notesBL.CreateNote(userId, notesModel);
                if(result != null)
                {
                    return Ok(new { sucess = true, message = "Note Created Successful", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Note Created Unsuccessful" });
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpDelete]
        [Route("Delete")]
        public IActionResult DeleteNotes(long NoteId)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "userID").Value);
                var delete = notesBL.DeleteNotes(NoteId);
                if (delete != null)
                {
                    return this.Ok(new { Success = true, message = "Notes Deleted Successfully" });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Notes Deleted Unsuccessful" });
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("Update")]
        public IActionResult UpdateNotes(NotesModel notesModel, long NoteId)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "userID").Value);
                var result = notesBL.UpdateNote(notesModel, NoteId);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Notes Updated Successfully", data = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "No Notes Found" });
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpGet]
        [Route("Get")]
        public IActionResult GetNote(long NotesId)
        {
            try
            {
                long note = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "userID").Value);
                List<NotesEntity> result = notesBL.GetNote(NotesId);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = " Note Display Successfully", data = result });
                }
                else
                    return this.BadRequest(new { Success = false, message = "Note not Available" });
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("Archive")]
        public IActionResult ArchiveNote(long NoteId)
        {
            try
            {
                long userid = Convert.ToInt32(User.Claims.FirstOrDefault(X => X.Type == "userID").Value);
                var result = notesBL.ArchiveNote(NoteId, userid);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Archived Successfully", data = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, Message = "Archived Unsuccessful" });
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("Pin")]
        public IActionResult PinNote(long NoteId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(p => p.Type == "userID").Value);
                var result = notesBL.PinNote(NoteId, userId);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Note Pinned Successfully", data = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Note Pinned Unsuccessful" });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("Trash")]
        public IActionResult TrashNote(long NotesId)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(t => t.Type == "userID").Value);
                var result = notesBL.TrashNote(NotesId, userId);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Trashed Successfully", data = result });
                }
                else
                {
                    return this.BadRequest(new { Success = false, message = "Trash Unuccessful" });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("Color")]
        public IActionResult NoteColor(long NoteId, string addcolor)
        {
            try
            {
                long userId = Convert.ToInt32(User.Claims.FirstOrDefault(x => x.Type == "userID").Value);
                var result = notesBL.NoteColor(NoteId, addcolor);
                if (result != null)
                {
                    return this.Ok(new { Success = true, message = "Color Added Successfully", data = result });
                }
                else
                    return this.BadRequest(new { Success = false, message = " Unsuccessful to add color" });
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPut]
        [Route("Image")]
        public IActionResult AddImage(string filePath, long noteId)
        {
            try
            {

                var result = notesBL.UploadImage(filePath, noteId);
                if (result != null)
                {
                    return Ok(new { success = true, message = "Uploaded Success", data = result });
                }
                else
                {
                    return BadRequest(new { success = false, message = "Upload Failed" });
                }
            }
            catch(Exception ex)
            {
                throw;
            }
        }
    }
}