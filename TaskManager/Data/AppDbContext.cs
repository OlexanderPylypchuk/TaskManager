using Microsoft.EntityFrameworkCore;
using TaskManager.Models;

namespace TaskManager.Data
{
    public class AppDbContext: DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options) 
        {
            
        }
        public DbSet<User> Users { get; set; }
        public DbSet<TaskOfUser> Tasks { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<User>().HasIndex(u=>u.Email).IsUnique();
            modelBuilder.Entity<User>().HasIndex(u => u.Username).IsUnique();
            //saving enums as string in db
            modelBuilder.Entity<TaskOfUser>().Property(t => t.Priority).HasConversion(v => v.ToString(), v => (PriorityTypes)Enum.Parse(typeof(PriorityTypes), v));
            modelBuilder.Entity<TaskOfUser>().Property(t => t.Status).HasConversion(v => v.ToString(), v => (StatusTypes)Enum.Parse(typeof(StatusTypes), v)); 
        }
    }
}
