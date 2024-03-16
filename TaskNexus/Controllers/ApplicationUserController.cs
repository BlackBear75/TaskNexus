using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Security.Claims;
using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.ViewModel.User;
using TaskNexus.Service.InterfaceService;

namespace TaskNexus.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    public class ApplicationUserController : Controller
    {
        private readonly IApplicationUserService _applicationUserService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        public ApplicationUserController(IApplicationUserService applicationUserService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _applicationUserService = applicationUserService;
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            var result = await _applicationUserService.Register(model);
            if (result.StatusCode ==Models.Enum.StatusCode.OK && result.Data)
            {
                return Ok("User registered successfully");
            }
            else
            {
                return BadRequest(result.Description);
            }
        }

        [HttpPost]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var result = await _applicationUserService.Login(model);
            var RES = _signInManager.IsSignedIn(User);
            if (result.StatusCode == Models.Enum.StatusCode.OK && result.Data)
            {
                return Ok("User logged in successfully");
            }
            else
            {
                return Unauthorized(result.Description);
            }
        }










        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _applicationUserService.GetUser(id);
            if (user.Data != null)
            {
                return Ok(user);
            }
            else
            {
                return BadRequest(user.Description);
            }
        }

        [HttpGet]
        [Route("GetUsers")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _applicationUserService.SelectUsers();
            if (users.StatusCode != Models.Enum.StatusCode.NullEntity)
            {
                return Ok(users);
            }
            else
            {
                return BadRequest(users.Description);
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, ApplicationUser user)
        {
            try
            {

                var updatedUser = await _applicationUserService.UpdateUser(id,user);
                if (updatedUser.StatusCode == Models.Enum.StatusCode.NullEntity)
                {

                    return BadRequest(updatedUser.Description);
                }
                else
                {
                    return Ok(updatedUser.Data);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update user: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
               
                    await _applicationUserService.DeleteUser(id);
                    return Ok("User deleted successfully");
                
               
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete user: {ex.Message}");
            }
        }
    }


}

