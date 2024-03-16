using TaskNexus.DAL.Interfaces;
using TaskNexus.DAL.Repository;
using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.Entity;
using TaskNexus.Models.Enum;
using TaskNexus.Models.Response;
using TaskNexus.Service.InterfaceService;

namespace TaskNexus.Service.ImplementationsService
{
    public class TaskFilterService : ITaskFilterService
    {
        private readonly ITaskFilterRepository _taskfillterRepository;

        public TaskFilterService(ITaskFilterRepository taskRepository)
        {
            _taskfillterRepository = taskRepository;
        }

        public async Task<IBaseResponse<IEnumerable<Task_Entity>>> TaskExecutionStatus(string userid)
        {
            var baseResponse = new BaseResponse<IEnumerable<Task_Entity>>();
            try
            {
                var tasks = await _taskfillterRepository.TaskExecutionStatus(userid);
                if (tasks == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Description = "TaskExecutionStatus no find tasks";
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
                    Description = $"[TaskExecutionStatus] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }

        public async Task<IBaseResponse<Task_Entity>> TaskNearestDeadline(string userid)
        {
            var baseResponse = new BaseResponse<Task_Entity>();
            try
            {
                var task = await _taskfillterRepository.TaskNearestDeadline(userid);
                if (task == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Description = "TaskNearestDeadline = null entity";

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
                    Description = $"[TaskNearestDeadline] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }

        public async Task<IBaseResponse<IEnumerable<Task_Entity>>> TaskNewest(string userid)
        {
            var baseResponse = new BaseResponse<IEnumerable<Task_Entity>>();
            try
            {
                var tasks = await _taskfillterRepository.TaskNewest(userid);
                if (tasks == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Description = "TaskNewest no find tasks";
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
                    Description = $"[TaskNewest] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }

        public async Task<IBaseResponse<IEnumerable<Task_Entity>>> TaskOldest(string userid)
        {
            var baseResponse = new BaseResponse<IEnumerable<Task_Entity>>();
            try
            {
                var tasks = await _taskfillterRepository.TaskOldest(userid);
                if (tasks == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Description = "TaskOldest no find tasks";
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
                    Description = $"[TaskOldest] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }

        public async Task<IBaseResponse<IEnumerable<Task_Entity>>> TaskPriority(string userid)
        {
            var baseResponse = new BaseResponse<IEnumerable<Task_Entity>>();
            try
            {
                var tasks = await _taskfillterRepository.TaskPriority(userid);
                if (tasks == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Description = "TaskPriority no find tasks";
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
                    Description = $"[TaskPriority] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }


        public async Task<IBaseResponse<EvaluationUser>> GetEvaluationUser(string userid)
        {
            var baseResponse = new BaseResponse<EvaluationUser>();
            try
            {
                var res = await _taskfillterRepository.UserTasks(userid);
             
                if (res.Count != 0)
                {
                    int taskCompleted = 0;

                   foreach (var task in res)
                    {
                        if (task.Status == Models.Enum.TaskStatus.Completed)
                        {
                            taskCompleted++;
                        }
                    }
                    

                   
                        var evaluationUser = await _taskfillterRepository.GetEvaluationUser(res.Count,taskCompleted);
                        baseResponse.StatusCode = StatusCode.OK;
                        baseResponse.Data = evaluationUser;
                        baseResponse.Description = "Good job";

                  
                }
                else
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Description = "List null";
                    baseResponse.Data = null;
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<EvaluationUser>()
                {
                    Description = $"[GetEvaluationUser] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }
    }
}
