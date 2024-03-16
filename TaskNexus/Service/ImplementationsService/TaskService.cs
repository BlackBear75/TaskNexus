using System;
using TaskNexus.DAL.Interfaces;
using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.Entity;
using TaskNexus.Models.Enum;
using TaskNexus.Models.Response;
using TaskNexus.Service.InterfaceService;

namespace TaskNexus.Service.ImplementationsService
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }


        public async Task<IBaseResponse<bool>> CreateTask(Task_Entity entity,string userid)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                if(entity==null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Description = "task = null";
                    baseResponse.Data = false;
                    return baseResponse;
                }
                entity.AssignedToId = userid;
                entity.CreatedAt = DateTime.Now;
                

                await _taskRepository.Create(entity);


                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Description = "Good job";
                baseResponse.Data = true;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[CreateTask] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }

        public async Task<IBaseResponse<bool>> DeleteTask(int id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            { 
                var task = await _taskRepository.Get(id);
                if (task == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Description = "deletetask = null entity";
                    baseResponse.Data = false;
                    return baseResponse;
                }
               

                await _taskRepository.Delete(task);


                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Description = "Good delete task";
                baseResponse.Data = true;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[DeleteTask] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }

       

        public async Task<IBaseResponse<Task_Entity>> GetTask(int id)
        {
            var baseResponse = new BaseResponse<Task_Entity>();
            try
            {
                var task = await _taskRepository.Get(id);
                if (task == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Description = "Gettask = null entity";
                   
                    return baseResponse;
                }


                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Description = "Good job";
                baseResponse.Data = task;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Task_Entity>()
                {
                    Description = $"[GetTask] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }

        public async Task<IBaseResponse<IEnumerable<Task_Entity>>> GetTasks(string userid)
        {
            var baseResponse = new BaseResponse<IEnumerable<Task_Entity>>();
            try
            {
                var tasks = await _taskRepository.Select(userid);
                if (tasks == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Description = "Gettasks no find tasks";
                    return baseResponse;
                }


                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Description = "Good job";
                baseResponse.Data = tasks;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<Task_Entity>>()
                {
                    Description = $"[GetTasks] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }
        
        public async Task<IBaseResponse<Task_Entity>> UpdateTask(int id,Task_Entity entity)
        {
            var baseResponse = new BaseResponse<Task_Entity>();
            try
            {
                var task = await _taskRepository.Get(id);
                if (task == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Description = "Updade task no entity";
                    return baseResponse;
                }
              
                task.Title = entity.Title;
                task.Description = entity.Description;
                task.Status = entity.Status;
                task.Priority = entity.Priority;
                task.Deadline = entity.Deadline;
                task.UpdatedAt = DateTime.UtcNow;
              

                await _taskRepository.Update(task);
                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Description = "Good job";
                baseResponse.Data = task;
            }
            catch (Exception ex)
            {
                return new BaseResponse<Task_Entity>()
                {
                    Description = $"[GetTasks] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }
    }
}
