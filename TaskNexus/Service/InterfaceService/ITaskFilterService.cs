using TaskNexus.Models.Entity;
using TaskNexus.Models.Response;

namespace TaskNexus.Service.InterfaceService
{
    public interface ITaskFilterService
    {


        Task<IBaseResponse<IEnumerable<Task_Entity>>> TaskExecutionStatus(string userid);

        Task<IBaseResponse<Task_Entity>> TaskNearestDeadline(string userid);

        Task<IBaseResponse<IEnumerable<Task_Entity>>> TaskOldest(string userid);

        Task<IBaseResponse<IEnumerable<Task_Entity>>> TaskNewest(string userid);

        Task<IBaseResponse<IEnumerable<Task_Entity>>> TaskPriority(string userid);

   


    }
}
