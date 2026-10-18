using Microsoft.EntityFrameworkCore;
using PersonalWeb.Api.Models;

namespace PersonalWeb.Api.EntityFramework
{
    public class PersonalWebApiContext : DbContext
    {
        public DbSet<Blog> Blogs { get; set; }
        public DbSet<Education> Education { get; set; }
        public DbSet<Experience> Experiences { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Profile> Profiles { get; set; }
        public DbSet<Engagement> Engagements { get; set; }
        public DbSet<Publication> Publications { get; set; }
        public DbSet<User> Users { get; set; }

        public PersonalWebApiContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();
            modelBuilder.Entity<Publication>()
                .HasIndex(p => p.Slug)
                .IsUnique();
        }
    }
}
