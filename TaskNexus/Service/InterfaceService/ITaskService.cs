using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.Entity;
using TaskNexus.Models.Response;

namespace TaskNexus.Service.InterfaceService
{
    public interface ITaskService
    {
        Task<IBaseResponse<bool>> CreateTask(Task_Entity entity, string userid);

        Task<IBaseResponse<Task_Entity>> GetTask(int id);

        Task<IBaseResponse<IEnumerable<Task_Entity>>> GetTasks(string userid);

        Task<IBaseResponse<bool>> DeleteTask(int id);

        Task<IBaseResponse<Task_Entity>> UpdateTask(int id,Task_Entity entity);

       
    }
}
