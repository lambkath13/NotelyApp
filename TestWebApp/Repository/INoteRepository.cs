using TestWebApp.Models;

namespace TestWebApp.Repository;

public interface INoteRepository
{
    NoteModel FindNoteById(Guid id);
    IEnumerable<NoteModel> GetAllNotes();
    void SaveNote(NoteModel noteModel);
    void DeleteNote(Guid id);
}