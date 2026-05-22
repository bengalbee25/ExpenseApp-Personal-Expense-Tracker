using Microsoft.EntityFrameworkCore;
using ExpenseTracker.Web.Models.Entities;

namespace ExpenseTracker.Web.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    public DbSet<AppUser> Users => Set<AppUser>();
    public DbSet<Transaction> Transactions => Set<Transaction>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<AppUser>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Name).HasMaxLength(120).IsRequired();
            entity.Property(x => x.Email).HasMaxLength(150).IsRequired();
            entity.Property(x => x.PasswordHash).IsRequired();
            entity.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Type).HasMaxLength(20).IsRequired();
            entity.Property(x => x.Category).HasMaxLength(100).IsRequired();
            entity.Property(x => x.Description).HasMaxLength(500);
            entity.Property(x => x.Amount).HasColumnType("decimal(18,2)");

            entity.HasOne(x => x.User)
                .WithMany(u => u.Transactions)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
