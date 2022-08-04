using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class NotesBL : INotesBL
    {
        private readonly INotesRL notesRL;

        public NotesBL(INotesRL notesRL)
        {
            this.notesRL = notesRL;
        }

        public NotesEntity CreateNote(long UserId, NotesModel notesModel)
        {
            try
            {
                return notesRL.CreateNote(UserId, notesModel);
            }
            catch(Exception)
            {
                throw;
            }
        }
        public NotesEntity DeleteNotes(long NoteId)
        {
            try
            {
                return notesRL.DeleteNotes(NoteId);
            }
            catch(Exception)
            {
                throw;
            }
        }

        public NotesEntity UpdateNote(NotesModel notesModel, long NoteId)
        {
            try
            {
                return notesRL.UpdateNote(notesModel,NoteId);
            }
            catch(Exception)
            {
                throw;
            }
        }
        public List<NotesEntity> GetNote(long NotesId)
        {
            try
            {
                return notesRL.GetNote(NotesId);
            }
            catch(Exception)
            {
                throw;
            }
        }
        public bool ArchiveNote(long NoteId, long userId)
        {
            try
            {
                return notesRL.ArchiveNote(NoteId, userId);
            }
            catch(Exception)
            {
                throw;
            }
        }
        public bool PinNote(long NoteId, long userId)
        {
            try
            {
                return notesRL.PinNote(NoteId, userId);
            }
            catch(Exception)
            {
                throw;
            }
        }
        public bool TrashNote(long NotesId, long userId)
        {
            try
            {
                return notesRL.TrashNote(NotesId, userId);
            }
            catch(Exception)
            {
                throw;
            }
        }
        public NotesEntity NoteColor(long NoteId, string addcolor)
        {
            try
            {
                return notesRL.NoteColor(NoteId, addcolor);
            }
            catch(Exception)
            {
                throw;
            }
        }
        public NotesEntity UploadImage(string filePath, long noteId)
        {
            try
            {
                return notesRL.UploadImage(filePath, noteId);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}