using Microsoft.AspNetCore.Mvc;
using TaskNexus.Models.ApplicationUser;
using TaskNexus.Service.InterfaceService;

namespace TaskNexus.Controllers
{
    [ApiController]
    public class ApplicationUserController : ControllerBase
    {
        private readonly IApplicationUserService _applicationUserService;
        public ApplicationUserController(IApplicationUserService applicationUserService)
        {
            _applicationUserService = applicationUserService;
        }


        [HttpPost]
        [Route("api/users")]
        public async Task<IActionResult> CreateUser(ApplicationUser user)
        {
            try
            {
                await _applicationUserService.CreateUser(user);
                return Ok("User created successfully");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create user: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(string id)
        {
            var user = await _applicationUserService.GetUser(id);
            if (user != null)
                return Ok(user);
            else
                return NotFound("User not found");
        }

        [HttpGet]
        [Route("api/users")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _applicationUserService.SelectUsers();
            return Ok(users);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(string id, ApplicationUser user)
        {
            try
            {
                if (id != user.Id)
                    return BadRequest("User ID mismatch");

                var updatedUser = await _applicationUserService.UpdateUser(id,user);
                if (updatedUser != null)
                    return Ok("User updated successfully");
                else
                    return NotFound("User not found");
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update user: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
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

