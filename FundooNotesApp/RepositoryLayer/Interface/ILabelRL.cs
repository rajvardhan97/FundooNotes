﻿using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface ILabelRL
    {
        public LabelEntity AddLabel(LabelModel labelModel);
        public IEnumerable<LabelEntity> GetAllLabel(long userId);
        public List<LabelEntity> Getlabel(long NotesId, long userId);
        public LabelEntity UpdateLabel(LabelModel labelModel, long labelID);
        public LabelEntity DeleteLabel(long labelID, long userId);
    }
}
