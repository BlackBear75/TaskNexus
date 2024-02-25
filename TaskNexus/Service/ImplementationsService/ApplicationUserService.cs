using TaskNexus.DAL.Interfaces;
using TaskNexus.DAL.Repository;
using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.Entity;
using TaskNexus.Models.Enum;
using TaskNexus.Models.Response;
using TaskNexus.Service.InterfaceService;

namespace TaskNexus.Service.ImplementationsService
{
    public class ApplicationUserService : IApplicationUserService
    {

        private readonly IApplicationUserRepository _applicactionuserRepository;

        public ApplicationUserService(IApplicationUserRepository applicactionuserRepository)
        {
            _applicactionuserRepository = applicactionuserRepository;
        }
        public async Task<IBaseResponse<bool>> CreateUser(ApplicationUser entity)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                if (entity == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Data = false;
                    return baseResponse;
                }

                await _applicactionuserRepository.Create(entity);


                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = true;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[CreateUser] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }

        public async Task<IBaseResponse<bool>> DeleteUser(string id)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var task = await _applicactionuserRepository.Get(id);
                if (task == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Data = false;
                    return baseResponse;
                }


                await _applicactionuserRepository.Delete(task);


                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = true;
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[DeleteUser] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }

        public async Task<IBaseResponse<ApplicationUser>> GetUser(string id)
        {
            var baseResponse = new BaseResponse<ApplicationUser>();
            try
            {
                var task = await _applicactionuserRepository.Get(id);
                if (task == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;

                    return baseResponse;
                }


                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = task;
            }
            catch (Exception ex)
            {
                return new BaseResponse<ApplicationUser>()
                {
                    Description = $"[GetUser] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }

        public async Task<IBaseResponse<IEnumerable<ApplicationUser>>> SelectUsers()
        {
            var baseResponse = new BaseResponse<IEnumerable<ApplicationUser>>();
            try
            {
                var tasks = await _applicactionuserRepository.Select();
                if (tasks == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    return baseResponse;
                }


                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = tasks;
            }
            catch (Exception ex)
            {
                return new BaseResponse<IEnumerable<ApplicationUser>>()
                {
                    Description = $"[SelectUsers] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }
        //Думать
        public async Task<IBaseResponse<ApplicationUser>> UpdateUser(string id,ApplicationUser entity)
        {
            var baseResponse = new BaseResponse<ApplicationUser>();
            try
            {
                var task = await _applicactionuserRepository.Get(id);
                if (task == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    return baseResponse;
                }

                task.UserName = entity.UserName;



                baseResponse.StatusCode = StatusCode.OK;
                baseResponse.Data = task;
            }
            catch (Exception ex)
            {
                return new BaseResponse<ApplicationUser>()
                {
                    Description = $"[UpdateUser] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }
    }
}

