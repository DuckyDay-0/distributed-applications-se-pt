using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace GameDevHelper.Models
{
    public class ApplicationDbContext : IdentityDbContext<User>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Project> Projects { get; set; }
        public DbSet<Feature> Features { get; set; }
        public DbSet<BugLog> BugLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Връзка между Project и Feature (1:M)
            modelBuilder.Entity<Feature>()
                .HasOne(f => f.Project)
                .WithMany(p => p.Features)
                .HasForeignKey(f => f.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Връзка между Project и BugLog (1:M)
            modelBuilder.Entity<BugLog>()
                .HasOne(b => b.Project)
                .WithMany(p => p.BugLogs)
                .HasForeignKey(b => b.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);

            // Връзка между Feature и User (1:M)
            modelBuilder.Entity<Feature>()
                .HasOne(f => f.AssignedTo)
                .WithMany(u => u.Features)
                .HasForeignKey(f => f.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull);

            // Връзка между BugLog и User (1:M)
            modelBuilder.Entity<BugLog>()
                .HasOne(b => b.AssignedTo)
                .WithMany(u => u.BugLogs)
                .HasForeignKey(b => b.AssignedToId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}
