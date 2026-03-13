using System.ComponentModel.DataAnnotations;

namespace TaskBoard.Models;

public enum Priority { Low, Medium, High }

public class TaskItem
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Title is required.")]
    [StringLength(200, ErrorMessage = "Title cannot exceed 200 characters.")]
    public string Title { get; set; } = string.Empty;

    [StringLength(1000)]
    public string? Description { get; set; }

    public Priority Priority { get; set; } = Priority.Medium;

    public bool IsCompleted { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? CompletedAt { get; set; }
}
