using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ILabelRL
    {
        public LabelEntity AddLabel(LabelModel labelModel);
        public IEnumerable<LabelEntity> GetLabel(long userId);
    }
}
