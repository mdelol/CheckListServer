using DAL.Models;
using Microsoft.EntityFrameworkCore;

namespace DAL.Contexts
{
    public class ProjectContext : DbContext
    {

        public ProjectContext(DbContextOptions<ProjectContext> options)
            : base(options)
        { }

        public DbSet<Task> Tasks { get; set; } 

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasAlternateKey(c => c.UserName)
                .HasName("Unique_UserName");
        }
    }
}