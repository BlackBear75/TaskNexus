using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.Entity;

namespace TaskNexus.DAL.Interfaces
{
    public interface ITaskFilterRepository
    {

        Task<List<Task_Entity>> TaskExecutionStatus(string userid);

        Task<Task_Entity> TaskNearestDeadline(string userid);

        Task<List<Task_Entity>> TaskOldest(string userid);

        Task<List<Task_Entity>> TaskNewest(string userid);

        Task<List<Task_Entity>> TaskPriority(string userid);

        Task<List<Task_Entity>> UserTasks(string userid);

        Task<EvaluationUser> GetEvaluationUser(int count,int taskcompleted);





    }
}
