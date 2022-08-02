using CommonLayer.Model;
using Microsoft.Extensions.Configuration;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class NotesRL : INotesRL
    {
        private readonly FundooContext fundooContext;

        public NotesRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }

        public NotesEntity CreateNote(long UserId,NotesModel notesModel)
        {
            try
            {
                NotesEntity notesEntity = new NotesEntity();
                notesEntity.Title = notesModel.Title;
                notesEntity.Description = notesModel.Description;
                notesEntity.Reminder = notesModel.Reminder;
                notesEntity.Color = notesModel.Color;
                notesEntity.Image = notesModel.Image;
                notesEntity.Archive = notesModel.Archive;
                notesEntity.Pin = notesModel.Pin;
                notesEntity.Trash = notesModel.Trash;
                notesEntity.Created = notesModel.Created;
                notesEntity.Edited = notesModel.Edited;
                notesEntity.UserId = UserId;

                fundooContext.NotesTable.Add(notesEntity);
                int result = fundooContext.SaveChanges();

                if (result != 0)
                {
                    return notesEntity;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public NotesEntity DeleteNotes(long NoteId)
        {
            try
            {
                var deleteNote = fundooContext.NotesTable.Where(x => x.NoteId == NoteId).FirstOrDefault();
                if (deleteNote != null)
                {
                    fundooContext.NotesTable.Remove(deleteNote);
                    fundooContext.SaveChanges();
                    return deleteNote;
                }
                else
                {
                    return null;
                }

            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotesEntity UpdateNote(NotesModel notesModel, long NoteId)
        {
            try
            {
                var update = fundooContext.NotesTable.Where(x => x.NoteId == NoteId).FirstOrDefault();
                if (update != null)
                {
                    update.Title = notesModel.Title;
                    update.Description = notesModel.Description;
                    update.Reminder = notesModel.Reminder;
                    update.Color = notesModel.Color;
                    update.Image = notesModel.Image;
                    fundooContext.NotesTable.Update(update);
                    fundooContext.SaveChanges();
                    return update;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public List<NotesEntity> GetNote(long NotesId)
        {
            try
            {
                var Note = fundooContext.NotesTable.Where(X => X.NoteId == NotesId).FirstOrDefault();
                if (Note != null)
                {
                    return fundooContext.NotesTable.Where(list => list.NoteId == NotesId).ToList();
                }
                return null;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotesEntity ArchiveNote(long NoteId, long userId)
        {
            try
            {
                var data = fundooContext.NotesTable.Where(A => A.NoteId == NoteId && A.UserId == userId).FirstOrDefault();
                if (data != null)
                {
                    if (data.Archive == false)
                    {
                        data.Archive = true;
                    }
                    else
                    {
                        data.Archive = false;
                    }
                    fundooContext.SaveChanges();
                    return data;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotesEntity PinNote(long NoteId, long userId)
        {
            var pin = fundooContext.NotesTable.Where(p => p.NoteId == NoteId && p.UserId == userId).FirstOrDefault();
            if (pin != null)
            {
                if (pin.Pin == false)
                {
                    pin.Pin = true;
                }
                else
                {
                    pin.Pin = false;
                }
                fundooContext.SaveChanges();
                return pin;
            }
            else
            {
                return null;
            }
        }
    }
}
