using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using CommonLayer.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualStudio.Services.Client.AccountManagement;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Account = CloudinaryDotNet.Account;

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
                    update.Created = notesModel.Created;
                    update.Edited = DateTime.Now;
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
                var Note = fundooContext.NotesTable.Where(x => x.NoteId == NotesId).FirstOrDefault();
                if (Note != null)
                {
                    return fundooContext.NotesTable.Where(list => list.NoteId == NotesId).ToList();
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

        public bool ArchiveNote(long NoteId, long userId)
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool PinNote(long NoteId, long userId)
        {
            try
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
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public bool TrashNote(long NotesId, long userId)
        {
            try
            {
                var trashed = fundooContext.NotesTable.Where(t => t.NoteId == NotesId && t.UserId == userId).FirstOrDefault();
                if (trashed != null)
                {
                    if (trashed.Trash == false)
                    {
                        trashed.Trash = true;
                    }
                    else
                    {
                        trashed.Trash = false;
                    }
                    fundooContext.SaveChanges();
                    return true;

                }
                else
                {
                    return false;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public NotesEntity NoteColor(long NoteId, string addcolor)
        {
            try
            {
                var note = fundooContext.NotesTable.Where(c => c.NoteId == NoteId).FirstOrDefault();
                if (note != null)
                {
                    if (addcolor != null)
                    {
                        note.Color = addcolor;
                        fundooContext.NotesTable.Update(note);
                        fundooContext.SaveChanges();
                        return note;
                    }
                    else
                    {
                        return null;
                    }
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

        public NotesEntity UploadImage(string filePath, long noteId)
        {
            try
            {
                var Notes = fundooContext.NotesTable.First(e => e.NoteId == noteId);
                if (Notes != null)
                {
                    Account account = new Account("dckjkypmk", "985331628442736", "BW90HnbQRtOxoayIUD9i1WtXq5I");
                    Cloudinary cloudinary = new Cloudinary(account);
                    ImageUploadParams uploadParams = new ImageUploadParams()
                    {
                        File = new FileDescription(filePath),
                        PublicId = Notes.Title
                    };

                    ImageUploadResult uploadResult = cloudinary.Upload(uploadParams);

                    Notes.Image = uploadResult.Url.ToString();
                    fundooContext.SaveChanges();
                    return Notes;
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
    }
}