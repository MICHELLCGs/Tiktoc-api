using System;
using System.ComponentModel.DataAnnotations;

public class UserAuthProvider
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public User User { get; set; }

    [Required]
    public string Provider { get; set; } // Ejemplo: "Google", "Facebook"
    [Required]
    public string ProviderId { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
