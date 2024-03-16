using Microsoft.EntityFrameworkCore;
using TaskNexus.DAL.DB;
using TaskNexus.DAL.Interfaces;
using TaskNexus.Models.ApplicationUser;

namespace TaskNexus.DAL.Repository
{
    public class ApplicationUserRepository : IApplicationUserRepository
    {
        private readonly ApplicationDbContext _db;
        public ApplicationUserRepository(ApplicationDbContext db)
        {
            _db=db;
        }
        public async Task<bool> Create(ApplicationUser entity)
        {
            try
            {
                _db.Users.Add(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(ApplicationUser entity)
        {
            try
            {
                _db.Tasks.RemoveRange(_db.Tasks.Where(t => t.AssignedToId == entity.Id));
                _db.Users.Remove(entity);
               
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<ApplicationUser> Get(string id)
        {

            return await _db.Users.FirstOrDefaultAsync(x => x.Id == id);
         
        }
      
        public async Task<List<ApplicationUser>> Select()
        {
            return await _db.Users.ToListAsync();
        }

        public async Task<ApplicationUser> Update(ApplicationUser entity)
        {
            try
            {
                _db.Users.Update(entity);
                await _db.SaveChangesAsync();
                return entity;
            }
            catch (Exception)
            {
                return null;
            }
        }
    }
}
