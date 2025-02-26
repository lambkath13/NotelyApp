using TestWebApp.Models;

namespace TestWebApp.Repository;

public class NoteRepository : INoteRepository
{
    private readonly List<NoteModel> _notes = new();

    public NoteRepository()
    {
        _notes = new List<NoteModel>();
    }

    public NoteModel FindNoteById(Guid id)
    {
        return _notes.FirstOrDefault(n => n.Id == id && !n.IsDeleted);
    }

    public IEnumerable<NoteModel> GetAllNotes()
    {
        return _notes.Where(n => !n.IsDeleted);
    }

    public void SaveNote(NoteModel noteModel)
    {
        var existingNote = _notes.FirstOrDefault(n => n.Id == noteModel.Id);
        if (existingNote != null)
        {
            existingNote.Subject = noteModel.Subject;
            existingNote.Detail = noteModel.Detail;
            existingNote.LastModified = DateTime.Now;
        }
        else
        {
            noteModel.Id = Guid.NewGuid();
            noteModel.CreateDate = DateTime.Now;
            noteModel.LastModified = noteModel.CreateDate;
            _notes.Add(noteModel);
        }
    }


    public void DeleteNote(Guid id)
    {
        var note = _notes.FirstOrDefault(n => n.Id == id);
        if (note != null)
        {
            note.IsDeleted = true;
        }
    }
}
