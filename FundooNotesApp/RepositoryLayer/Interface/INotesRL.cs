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
        public IEnumerable<NotesEntity> GetAllNotes(long userId);
        public List<NotesEntity> GetNote(long NotesId);
        public bool ArchiveNote(long NoteId, long userId);
        public bool PinNote(long NoteId, long userId);
        public bool TrashNote(long NotesId, long userId);
        public NotesEntity NoteColor(long NoteId, string addcolor);
        public NotesEntity UploadImage(string filePath, long noteId);
    }
}
