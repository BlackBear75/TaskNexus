using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using TaskNexus.DAL.Interfaces;
using TaskNexus.DAL.Repository;
using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.Entity;
using TaskNexus.Models.Enum;
using TaskNexus.Models.Response;
using TaskNexus.Models.ViewModel.User;
using TaskNexus.Service.InterfaceService;

namespace TaskNexus.Service.ImplementationsService
{
    public class ApplicationUserService : IApplicationUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IApplicationUserRepository _applicactionuserRepository;

        public ApplicationUserService(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IApplicationUserRepository applicactionuserRepository)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _applicactionuserRepository = applicactionuserRepository;
        }

        public async Task<IBaseResponse<bool>> Register(RegisterViewModel entity)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var existingUser = await _userManager.FindByEmailAsync(entity.Email);
                if (existingUser != null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Description = "User with this email already exists";
                    baseResponse.Data = false;
                    return baseResponse;
                }

                var user = new ApplicationUser
                {
                    UserName = entity.UserName,
                    Email = entity.Email
                };

                var result = await _userManager.CreateAsync(user, entity.Password);
                if (result.Succeeded)
                {
                    baseResponse.StatusCode = StatusCode.OK;
                    baseResponse.Data = true;
                    baseResponse.Description = "User registered successfully";
                }
                else
                {
                    baseResponse.StatusCode = StatusCode.BadRequest;
                    baseResponse.Description = "User registration failed";
                    baseResponse.Data = false;
                }
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
                    baseResponse.Description = "NullEntity";
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
                if (tasks.Count == 0)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Description = "NullEntity";
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
       

        public async Task<IBaseResponse<ApplicationUser>> UpdateUser(string id,ApplicationUser entity)
        {
            var baseResponse = new BaseResponse<ApplicationUser>();
            try
            {
                var user = await _applicactionuserRepository.Get(id);
                if (user == null)
                {
                    baseResponse.StatusCode = StatusCode.NullEntity;
                    baseResponse.Description = "Not find user his id";
                    return baseResponse;
                }

                user.UserName = entity.UserName;

                //work

             var res =  await _applicactionuserRepository.Update(user);
                if (res!=null)
                {
                 baseResponse.StatusCode = StatusCode.OK;
                    baseResponse.Data = user;
                    return baseResponse;
                }
                else
                {
                    baseResponse.StatusCode = StatusCode.NoUpdate;
                    baseResponse.Description = "NoUpdate";
                    return baseResponse;
                }
             
            }
            catch (Exception ex)
            {
                return new BaseResponse<ApplicationUser>()
                {
                    Description = $"[UpdateUser] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
          
        }

        public async Task<IBaseResponse<bool>> Login(LoginViewModel entity)
        {
            var baseResponse = new BaseResponse<bool>();
            try
            {
                var user = await _userManager.FindByEmailAsync(entity.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user, entity.Password, false, lockoutOnFailure: false);

                    if (result.Succeeded)
                    {
                        baseResponse.StatusCode = StatusCode.OK;
                        baseResponse.Data = true;
                        baseResponse.Description = "User logged in successfully";
                    }
                    else
                    {
                        baseResponse.StatusCode = StatusCode.Unauthorized;
                        baseResponse.Description = "Invalid email or password";
                        baseResponse.Data = false;
                    }
                }
                else
                {
                    baseResponse.StatusCode = StatusCode.Unauthorized;
                    baseResponse.Description = "Invalid email or password";
                    baseResponse.Data = false;
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse<bool>()
                {
                    Description = $"[Login] : {ex.Message}",
                    StatusCode = StatusCode.InternalServerError
                };
            }
            return baseResponse;
        }
    }
}

