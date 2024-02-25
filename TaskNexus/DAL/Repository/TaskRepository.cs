using Microsoft.EntityFrameworkCore;
using TaskNexus.DAL.DB;
using TaskNexus.DAL.Interfaces;
using TaskNexus.Models.Entity;

namespace TaskNexus.DAL.Repository
{
    public class TaskRepository : ITaskRepository
    {
        private readonly ApplicationDbContext _db;

        public TaskRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> Create(Task_Entity entity)
        {
            try
            {
                _db.Tasks.Add(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> Delete(Task_Entity entity)
        {
            try
            {
                _db.Tasks.Remove(entity);
                await _db.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<Task_Entity> Get(int id)
        {
            return await _db.Tasks.FindAsync(id);
        }

        public async Task<List<Task_Entity>> Select()
        {
            return await _db.Tasks.ToListAsync();
        }

        public async Task<Task_Entity> Update(Task_Entity entity)
        {
            try
            {
                _db.Tasks.Update(entity);
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
