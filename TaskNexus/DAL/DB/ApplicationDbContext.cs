using Microsoft.EntityFrameworkCore;
using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.Entity;

namespace TaskNexus.DAL.DB
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext) :base(dbContext)
        {
           Database.EnsureCreated();
        }

       public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Task_Entity> Tasks { get; set; }

       
    }
}
