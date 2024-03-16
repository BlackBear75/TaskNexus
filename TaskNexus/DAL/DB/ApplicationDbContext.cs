using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.Entity;

namespace TaskNexus.DAL.DB
{
    public class ApplicationDbContext: IdentityDbContext<ApplicationUser, IdentityRole, string>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> dbContext) :base(dbContext)
        {
        
        }

       public DbSet<ApplicationUser> Users { get; set; }

        public DbSet<Task_Entity> Tasks { get; set; }

        public DbSet<EvaluationUser> evaluationUsers { get; set; }

       
    }
}
