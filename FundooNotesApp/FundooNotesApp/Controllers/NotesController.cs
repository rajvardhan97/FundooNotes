using BusinessLayer.Interface;
using CommonLayer.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FundooNotesApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class NotesController : ControllerBase
    {
        private readonly INotesBL notesBL;
        private readonly IMemoryCache memoryCache;
        private readonly IDistributedCache distributedCache;
        private readonly FundooContext fundooContext;
        public NotesController(INotesBL notesBL, FundooContext fundooContext, IMemoryCache memoryCache, IDistributedCache distributedCache)
        {
            this.notesBL = notesBL;
            this.memoryCache = memoryCache;
            this.distributedCache = distributedCache;
            this.fundooContext = fundooContext;
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

        [HttpGet("GetAll")]
        public IActionResult GetAllNotes(long userId)
        {
            try
            {
                var notes = notesBL.GetAllNotes(userId);
                if (notes != null)
                {
                    return Ok(new { Success = true, message = "All notes found Successfully", data = notes });

                }
                else
                {
                    return BadRequest(new { Success = false, message = "No Notes Found" });
                }
            }
            catch (Exception ex)
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

        [HttpGet]
        [Route("Redis")]
        public async Task<IActionResult> GetAllCustomersUsingRedisCache()
        {
            var cacheKey = "NotesList";
            string serializedNotesList;
            var NotesList = new List<NotesEntity>();
            var redisNotesList = await distributedCache.GetAsync(cacheKey);
            if (redisNotesList != null)
            {
                serializedNotesList = Encoding.UTF8.GetString(redisNotesList);
                NotesList = JsonConvert.DeserializeObject<List<NotesEntity>>(serializedNotesList);
            }
            else
            {
                NotesList = fundooContext.NotesTable.ToList();
                serializedNotesList = JsonConvert.SerializeObject(NotesList);
                redisNotesList = Encoding.UTF8.GetBytes(serializedNotesList);
                var options = new DistributedCacheEntryOptions()
                    .SetAbsoluteExpiration(DateTime.Now.AddMinutes(10))
                    .SetSlidingExpiration(TimeSpan.FromMinutes(2));
                await distributedCache.SetAsync(cacheKey, redisNotesList, options);
            }
            return Ok(NotesList);
        }


    }
}