using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.Response;

namespace TaskNexus.Service.InterfaceService
{
    public interface IApplicationUserService
    {
        Task<IBaseResponse<bool>> CreateUser(ApplicationUser entity);

        Task<IBaseResponse<ApplicationUser>> GetUser(string id);

        Task<IBaseResponse<IEnumerable<ApplicationUser>>> SelectUsers();

        Task<IBaseResponse<bool>> DeleteUser(string id);

        Task<IBaseResponse<ApplicationUser>> UpdateUser(string id,ApplicationUser entity);
    }
}
