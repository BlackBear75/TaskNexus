using TaskNexus.Models.Entity;

namespace TaskNexus.DAL.Interfaces
{
    public interface ITaskRepository
    {
        Task<bool> Create(Task_Entity entity);

        Task<Task_Entity> Get(int id);

        Task<List<Task_Entity>> Select(string userid);

        Task<bool> Delete(Task_Entity entity);
      

        Task<Task_Entity> Update(Task_Entity entity);
    }


}

