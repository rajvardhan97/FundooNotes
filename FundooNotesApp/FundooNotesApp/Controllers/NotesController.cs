﻿using BusinessLayer.Interface;
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
        [Route("Read")]
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
    }
}