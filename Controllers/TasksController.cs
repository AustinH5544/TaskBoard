using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TaskBoard.Data;
using TaskBoard.Models;

namespace TaskBoard.Controllers;

public class TasksController : Controller
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>Returns the task board index with all tasks sorted by creation date descending.</summary>
    public async Task<IActionResult> Index()
    {
        var tasks = await _context.Tasks
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        return View(tasks);
    }

    /// <summary>Returns a filtered partial view of tasks for AJAX requests.</summary>
    /// <param name="status">Filter value: "all", "active", or "completed".</param>
    public async Task<IActionResult> Filter(string status = "all")
    {
        var query = _context.Tasks.AsQueryable();

        query = status switch
        {
            "active"    => query.Where(t => !t.IsCompleted),
            "completed" => query.Where(t => t.IsCompleted),
            _           => query
        };

        var tasks = await query.OrderByDescending(t => t.CreatedAt).ToListAsync();
        return PartialView("_TaskTable", tasks);
    }

    /// <summary>Returns the Create task form.</summary>
    public IActionResult Create() => View();

    /// <summary>Handles task creation form submission.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(TaskItem task)
    {
        if (!ModelState.IsValid) return View(task);

        task.CreatedAt = DateTime.UtcNow;
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    /// <summary>Returns the Edit form for an existing task.</summary>
    /// <param name="id">The task ID to edit.</param>
    public async Task<IActionResult> Edit(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return NotFound();
        return View(task);
    }

    /// <summary>Handles task edit form submission.</summary>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, TaskItem task)
    {
        if (id != task.Id) return BadRequest();
        if (!ModelState.IsValid) return View(task);

        _context.Tasks.Update(task);
        await _context.SaveChangesAsync();
        return RedirectToAction(nameof(Index));
    }

    /// <summary>Toggles task completion via AJAX; returns JSON with updated state.</summary>
    /// <param name="id">The task ID to toggle.</param>
    [HttpPost]
    public async Task<IActionResult> ToggleComplete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return Json(new { success = false });

        task.IsCompleted = !task.IsCompleted;
        task.CompletedAt = task.IsCompleted ? DateTime.UtcNow : null;
        await _context.SaveChangesAsync();

        return Json(new { success = true, isCompleted = task.IsCompleted });
    }

    /// <summary>Deletes a task inline via AJAX; returns JSON confirming removal.</summary>
    /// <param name="id">The task ID to delete.</param>
    [HttpPost]
    public async Task<IActionResult> Delete(int id)
    {
        var task = await _context.Tasks.FindAsync(id);
        if (task == null) return Json(new { success = false });

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
        return Json(new { success = true });
    }
}
