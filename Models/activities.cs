using System;
using System.ComponentModel.DataAnnotations;

public class Activity
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public User User { get; set; }

    [Required]
    public int SessionId { get; set; }
    public Session Session { get; set; }

    [Required]
    public string Action { get; set; }

    public string Description { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
