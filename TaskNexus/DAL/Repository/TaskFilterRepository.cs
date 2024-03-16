using Microsoft.EntityFrameworkCore;
using System.Linq;
using TaskNexus.DAL.DB;
using TaskNexus.DAL.Interfaces;
using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.Entity;

namespace TaskNexus.DAL.Repository
{
    public class TaskFilterRepository : ITaskFilterRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TaskFilterRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<EvaluationUser> GetEvaluationUser(int count, int taskcompleted)
        {

            if (count - taskcompleted <= 3)
            {
                return await _dbContext.evaluationUsers.FirstOrDefaultAsync(x => x.Id == 2);
            }
            if (count-taskcompleted<=10)
            {
                return  await _dbContext.evaluationUsers.FirstOrDefaultAsync(x => x.Id == 1);

            }

            return await  _dbContext.evaluationUsers.FirstOrDefaultAsync(x => x.Id == 3);



        }

        public async Task<List<Task_Entity>> TaskExecutionStatus(string userid)
        {
            return await _dbContext.Tasks
            .Where(t => t.AssignedToId == userid && t.Status == TaskNexus.Models.Enum.TaskStatus.Completed)
            .ToListAsync();
        }

        public async Task<Task_Entity> TaskNearestDeadline(string userid)
        {
            return await _dbContext.Tasks
            .Where(t => t.AssignedToId == userid && t.Deadline >= DateTime.Now)
            .OrderBy(t => t.Deadline)
            .FirstOrDefaultAsync();
        }

        public async Task<List<Task_Entity>> TaskNewest(string userid)
        {
            return await _dbContext.Tasks
            .Where(t => t.AssignedToId == userid)
            .OrderByDescending(t => t.CreatedAt)
            .ToListAsync();
        }

        public async Task<List<Task_Entity>> TaskOldest(string userid)
        {
            return await _dbContext.Tasks
             .Where(t => t.AssignedToId == userid)
             .OrderBy(t => t.CreatedAt)
             .ToListAsync();
        }

        public async Task<List<Task_Entity>> TaskPriority(string userid)
        {
            return await _dbContext.Tasks
            .Where(t => t.AssignedToId == userid)
            .OrderByDescending(t => t.Priority)
            .ToListAsync();
        }

        public async Task<List<Task_Entity>> UserTasks(string userid)
        {
            return await _dbContext.Tasks
           .Where(t => t.AssignedToId == userid).ToListAsync();
        }
    }
}
