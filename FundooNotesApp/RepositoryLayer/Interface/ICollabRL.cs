using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ICollabRL
    {
        public CollabEntity AddCollab(CollabModel collabModel);
        public string RemoveCollab(long collabId, long userId);
    }
}
