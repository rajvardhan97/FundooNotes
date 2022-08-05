﻿using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Interface
{
     public interface ILabelBL
    {
        public LabelEntity AddLabel(LabelModel labelModel);
        public IEnumerable<LabelEntity> GetLabel(long userId);
    }
}
