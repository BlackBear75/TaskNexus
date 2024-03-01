using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.Response;
using TaskNexus.Models.ViewModel.User;

namespace TaskNexus.Service.InterfaceService
{
    public interface IApplicationUserService
    {
        Task<IBaseResponse<bool>> Register(RegisterViewModel entity);
        Task<IBaseResponse<bool>> Login(LoginViewModel entity);


        Task<IBaseResponse<ApplicationUser>> GetUser(string id);

        Task<IBaseResponse<IEnumerable<ApplicationUser>>> SelectUsers();

        Task<IBaseResponse<bool>> DeleteUser(string id);

        Task<IBaseResponse<ApplicationUser>> UpdateUser(string id,ApplicationUser entity);
    }
}
