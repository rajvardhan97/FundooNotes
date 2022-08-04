using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class CollabBL : ICollabBL
    {
        private readonly ICollabRL collabRL;
        public CollabBL(ICollabRL collabRL)
        {
            this.collabRL = collabRL;
        }
        public CollabEntity AddCollab(CollabModel collabModel)
        {
            try
            {
                return collabRL.AddCollab(collabModel);
            }
            catch(Exception)
            {
                throw;
            }
        }
        public string RemoveCollab(long collabId, long userId)
        {
            try
            {
                return collabRL.RemoveCollab(collabId, userId);
            }
            catch(Exception)
            {
                throw;
            }
        }
    }
}
