using Microsoft.EntityFrameworkCore;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // Tablas principales
    public DbSet<User> Users { get; set; }
    public DbSet<UserAuthProvider> UserAuthProviders { get; set; }
    public DbSet<UserProfile> UserProfiles { get; set; }
    public DbSet<Session> Sessions { get; set; }
    public DbSet<Activity> Activities { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<LoginAttempt> LoginAttempts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Relación 1 a 1 entre User y UserProfile
        modelBuilder.Entity<User>()
            .HasOne(u => u.UserProfile)
            .WithOne(p => p.User)
            .HasForeignKey<UserProfile>(p => p.UserId);

        // Relación 1 a muchos entre User y UserAuthProvider
        modelBuilder.Entity<UserAuthProvider>()
            .HasOne(uap => uap.User)
            .WithMany(u => u.UserAuthProviders)
            .HasForeignKey(uap => uap.UserId);

        // Relación 1 a muchos entre User y Session
        modelBuilder.Entity<Session>()
            .HasOne(s => s.User)
            .WithMany(u => u.Sessions)
            .HasForeignKey(s => s.UserId);

        // Relación 1 a muchos entre User y LoginAttempt
        modelBuilder.Entity<LoginAttempt>()
            .HasOne(la => la.User)
            .WithMany(u => u.LoginAttempts)
            .HasForeignKey(la => la.UserId);

        // Relación 1 a muchos entre User y UserRole
        modelBuilder.Entity<UserRole>()
            .HasOne(ur => ur.User)
            .WithMany(u => u.UserRoles)
            .HasForeignKey(ur => ur.UserId);

        // Relación 1 a muchos entre Session y Activity
        modelBuilder.Entity<Activity>()
            .HasOne(a => a.Session)
            .WithMany()
            .HasForeignKey(a => a.SessionId);

        // Relación 1 a muchos entre User y Activity
        modelBuilder.Entity<Activity>()
            .HasOne(a => a.User)
            .WithMany()
            .HasForeignKey(a => a.UserId);

        // Configuración de índices
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.PhoneNumber)
            .IsUnique();

        modelBuilder.Entity<Session>()
            .HasIndex(s => s.SessionToken)
            .IsUnique();

        modelBuilder.Entity<UserProfile>()
            .HasIndex(p => p.Username)
            .IsUnique();

        modelBuilder.Entity<UserAuthProvider>()
            .HasIndex(uap => new { uap.Provider, uap.ProviderId })
            .IsUnique();

        modelBuilder.Entity<LoginAttempt>()
            .HasIndex(la => new { la.UserId, la.IpAddress, la.Success });
    }
}
