namespace TestWebApp.Models;

public class NoteModel
{
    public Guid Id { get; set; }
    public string? Subject { get; set; }
    public string? Detail { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime LastModified { get; set; }
    public bool IsDeleted { get; set; }
}