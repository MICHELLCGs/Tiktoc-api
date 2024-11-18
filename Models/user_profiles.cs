using System;
using System.ComponentModel.DataAnnotations;

public class UserProfile
{
    [Key]
    public int Id { get; set; }

    [Required]
    public int UserId { get; set; }
    public User User { get; set; }

    [Required]
    public string Username { get; set; }

    public DateTime? BirthDate { get; set; }
    public string Country { get; set; }
    public string ProfilePicture { get; set; }
    public string Gender { get; set; } // Enum: "male", "female", etc.

    public string Level { get; set; } = "Principiante";
    public int TotalSurveys { get; set; } = 0;
    public int TotalCoins { get; set; } = 0;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
}
