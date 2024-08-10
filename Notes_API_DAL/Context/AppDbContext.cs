using Notes_API_CORE.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Notes_API_DAL.Context
{
    public class AppDbContext : DbContext
    {
        public AppDbContext() : base()
        {

        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base (options) 
        {

        }
        public DbSet<User> Users { get; set; }
        public DbSet<Note> Notes { get; set; }
        public DbSet<UserNote> UsersNotes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //Aplica las configuraciones
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        }
    }
}
