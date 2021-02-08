using Microsoft.EntityFrameworkCore;
using WebService.Infrastructure.Entity;

namespace WebService.Infrastructure.Context
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> User { get; set; }
        public DbSet<TodoRecord> TodoRecord { get; set; }

        public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TodoRecord>()
            .HasOne(p => p.User)
            .WithMany(b => b.TodoRecords)
            .HasForeignKey(p => p.IdUser);
        }
    }
}
