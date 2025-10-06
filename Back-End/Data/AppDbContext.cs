using Microsoft.EntityFrameworkCore;
using WeatherLens.Entities;

namespace WeatherLens.Data;

/// <summary>
/// Represents the Entity Framework Core database context for the WeatherLens application.
/// </summary>
/// <remarks>
/// This context manages all entities, relationships, and persistence logic for
/// user accounts, locations, weather variables, queries, and results.
/// </remarks>
public sealed class AppDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class
    /// using the provided options (used for dependency injection).
    /// </summary>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    // --------------------------------------------------------------------
    // DbSets
    // --------------------------------------------------------------------

    public DbSet<User> Users => Set<User>();
    public DbSet<Location> Locations => Set<Location>();
    public DbSet<WeatherVariable> Variables => Set<WeatherVariable>();
    public DbSet<WeatherQuery> Queries => Set<WeatherQuery>();
    public DbSet<WeatherQueryVariable> QueryVariables => Set<WeatherQueryVariable>();
    public DbSet<WeatherResult> Results => Set<WeatherResult>();

    // --------------------------------------------------------------------
    // Model configuration
    // --------------------------------------------------------------------
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // ---------------------------
        // User
        // ---------------------------
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(u => u.Name)
                  .HasMaxLength(100)
                  .IsRequired();

            entity.Property(u => u.Email)
                  .HasMaxLength(150)
                  .IsRequired();

            entity.Property(u => u.Role)
                  .HasMaxLength(50);

            entity.Property(u => u.CreatedAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // ---------------------------
        // Location
        // ---------------------------
        modelBuilder.Entity<Location>(entity =>
        {
            entity.Property(l => l.Name)
                  .HasMaxLength(150)
                  .IsRequired();

            entity.Property(l => l.CreatedAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // ---------------------------
        // WeatherVariable
        // ---------------------------
        modelBuilder.Entity<WeatherVariable>(entity =>
        {
            entity.Property(v => v.Name)
                  .HasMaxLength(50)
                  .IsRequired();

            entity.Property(v => v.Unit)
                  .HasMaxLength(30)
                  .IsRequired();

            entity.Property(v => v.Description)
                  .HasMaxLength(255)
                  .IsRequired();

            entity.Property(v => v.CreatedAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        // ---------------------------
        // WeatherQuery
        // ---------------------------
        modelBuilder.Entity<WeatherQuery>(entity =>
        {
            entity.Property(q => q.CreatedAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            // Foreign key: Location → Queries
            entity.HasOne(q => q.Location)
                  .WithMany()
                  .HasForeignKey(q => q.LocationId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------------------------
        // WeatherQueryVariable (Join)
        // ---------------------------
        modelBuilder.Entity<WeatherQueryVariable>(entity =>
        {
            entity.Property(qv => qv.CreatedAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(qv => qv.Query)
                  .WithMany()
                  .HasForeignKey(qv => qv.QueryId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(qv => qv.Variable)
                  .WithMany()
                  .HasForeignKey(qv => qv.VariableId)
                  .OnDelete(DeleteBehavior.Cascade);
        });

        // ---------------------------
        // WeatherResult
        // ---------------------------
        modelBuilder.Entity<WeatherResult>(entity =>
        {
            entity.Property(r => r.CreatedAt)
                  .HasDefaultValueSql("CURRENT_TIMESTAMP");

            entity.HasOne(r => r.Query)
                  .WithMany()
                  .HasForeignKey(r => r.QueryId)
                  .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(r => r.Variable)
                  .WithMany()
                  .HasForeignKey(r => r.VariableId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
