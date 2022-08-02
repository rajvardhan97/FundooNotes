using CommonLayer.Model;
using RepositoryLayer.Entity;
using System;
using System.Collections.Generic;
using System.Text;

namespace RepositoryLayer.Interface
{
    public interface INotesRL
    {
        public NotesEntity CreateNote(long UserId, NotesModel notesModel);
        public NotesEntity DeleteNotes(long NoteId);
        public NotesEntity UpdateNote(NotesModel notesModel, long NoteId);
        public List<NotesEntity> GetNote(long NotesId);
    }
}
