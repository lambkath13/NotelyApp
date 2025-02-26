using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TestWebApp.Models;
using TestWebApp.Repository;

namespace TestWebApp.Controllers;
public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly INoteRepository _noteRepository;

    public HomeController(ILogger<HomeController> logger, INoteRepository noteRepository)
    {
        _logger = logger;
        _noteRepository = noteRepository;
    }

    public IActionResult Index(string searchQuery = "")
    {
        var notes = _noteRepository.GetAllNotes()
            .Where(n => !n.IsDeleted && (string.IsNullOrEmpty(searchQuery) || n.Subject.Contains(searchQuery, StringComparison.OrdinalIgnoreCase)));
        return View(notes);
    }

    public IActionResult NoteDetail(Guid id)
    {
        var note = _noteRepository.FindNoteById(id);
        if (note == null || note.IsDeleted) return NotFound();
        return View(note);
    }
    
    [HttpGet]
    public IActionResult NoteEditor(Guid? id)
    {
        if (id.HasValue)
        {
            var note = _noteRepository.FindNoteById(id.Value);
            if (note == null || note.IsDeleted) return NotFound();
            return View(note);
        }
        return View(new NoteModel());
    }

    [HttpPost]
    public IActionResult NoteEditor(NoteModel noteModel)
    {
        if (ModelState.IsValid)
        {
            var date = DateTime.Now;
            if (noteModel.Id == Guid.Empty)
            {
                noteModel.Id = Guid.NewGuid();
                noteModel.CreateDate = date;
                noteModel.LastModified = date;
                _noteRepository.SaveNote(noteModel);
            }
            else
            {
                var note = _noteRepository.FindNoteById(noteModel.Id);
                if (note == null || note.IsDeleted) return NotFound();
                note.LastModified = date;
                note.Subject = noteModel.Subject;
                note.Detail = noteModel.Detail;
            }
            return RedirectToAction("Index");
        }
        return View(noteModel);
    }

    public IActionResult DeleteNote(Guid id)
    {
        var note = _noteRepository.FindNoteById(id);
        if (note == null || note.IsDeleted) return NotFound();
        note.IsDeleted = true;
        return RedirectToAction("Index");
    }


    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}