using System;
using System.ComponentModel.DataAnnotations;

public class LoginAttempt
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public User User { get; set; }

    public string IpAddress { get; set; }
    public string UserAgent { get; set; }
    public bool Success { get; set; }
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
