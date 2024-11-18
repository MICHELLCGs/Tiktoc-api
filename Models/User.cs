using System;
using System.ComponentModel.DataAnnotations;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required]
    [EmailAddress]
    public string Email { get; set; }

    public string PhoneNumber { get; set; }

    [Required]
    public string PasswordHash { get; set; }

    public string OtpCode { get; set; }
    public DateTime? OtpExpiration { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? LastLogin { get; set; }

    public bool IsActive { get; set; } = true;
    public bool IsVerified { get; set; } = false;

    // Relaciones
    public UserProfile UserProfile { get; set; }
    public ICollection<UserAuthProvider> UserAuthProviders { get; set; }
    public ICollection<Session> Sessions { get; set; }
    public ICollection<LoginAttempt> LoginAttempts { get; set; }
    public ICollection<UserRole> UserRoles { get; set; }
}
