using CommonLayer.Model;
using RepositoryLayer.Context;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RepositoryLayer.Service
{
    public class CollabRL : ICollabRL
    {
        public readonly FundooContext fundooContext;

        public CollabRL(FundooContext fundooContext)
        {
            this.fundooContext = fundooContext;
        }
        public CollabEntity AddCollab(CollabModel collabModel)
        {
            try
            {
                var noteData = fundooContext.NotesTable.Where(x => x.NoteId == collabModel.NoteId).FirstOrDefault();
                var userData = fundooContext.UserTable.Where(x => x.Email == collabModel.CollabEmail).FirstOrDefault();
                if (noteData != null && userData != null)
                {
                    CollabEntity collabEntity = new CollabEntity()
                    {
                        CollabEmail = collabModel.CollabEmail,
                        NoteId = collabModel.NoteId,
                        UserId = userData.UserId
                    };
                    fundooContext.CollabTable.Add(collabEntity);
                    var result = fundooContext.SaveChanges();
                    return collabEntity;
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

        public string RemoveCollab(long collabId, long userId)
        {
            try
            {
                var collab = fundooContext.CollabTable.Where(X => X.CollabID == collabId).FirstOrDefault();
                if (collab != null)
                {
                    fundooContext.CollabTable.Remove(collab);
                    fundooContext.SaveChanges();
                    return "Removed Successfully";
                }
                else
                {
                    return null;
                }
            }
            catch(Exception)
            {
                throw;
            }
        }

        public IEnumerable<CollabEntity> GetCollab(long noteId, long userId)
        {
            try
            {
                var result = fundooContext.CollabTable.ToList().Where(x => x.NoteId == noteId);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
