using BusinessLayer.Interface;
using CommonLayer.Model;
using RepositoryLayer.Entity;
using RepositoryLayer.Interface;
using System;
using System.Collections.Generic;
using System.Text;

namespace BusinessLayer.Service
{
    public class LabelBL : ILabelBL
    {
        private readonly ILabelRL labelRL;
        public LabelBL(ILabelRL labelRL)
        {
            this.labelRL = labelRL;
        }
        public LabelEntity AddLabel(LabelModel labelModel)
        {
            try
            {
                return labelRL.AddLabel(labelModel);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
