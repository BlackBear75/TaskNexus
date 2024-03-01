using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.Entity;
using TaskNexus.Service.ImplementationsService;
using TaskNexus.Service.InterfaceService;

namespace TaskNexus.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskService _taskService;
      
        public TaskController(ITaskService taskService, UserManager<ApplicationUser> userManager)
        {
            _taskService = taskService;
            _userManager = userManager;
        }


        [HttpPost]
        [Route("CreateTask")]

        public async Task<IActionResult> CreateTask(Task_Entity task)
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
               if(user ==null)
                {
                    return BadRequest("User not found");

                }

                var res = await _taskService.CreateTask(task, user.Id);

                if (res.Data)
                {
                    return Ok("Task created successfully");
                }
                else
                {
                    return BadRequest(res.Description);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to create task: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetTask(int id)
        {
            var task = await _taskService.GetTask(id);
            if (task.Data != null)
            {
                return Ok(task);
            }
            else
            {
                return BadRequest(task.Description);
            }
        }

        [HttpGet]
        [Route("GetTasks")]
        
        public async Task<IActionResult> GetTasks()
        {
            var user = await _userManager.GetUserAsync(User);
            var tasks = await _taskService.GetTasks(user.Id);
            if (tasks.StatusCode != Models.Enum.StatusCode.NullEntity)
            {
                return Ok(tasks);
            }
            else
            {
                return BadRequest(tasks.Description);
            }
        }

        [HttpPut("{id}")]
        
        public async Task<IActionResult> UpdateTask(int id, Task_Entity task)
        {
            try
            {

                var updatedTask = await _taskService.UpdateTask(id, task);
                if (updatedTask.StatusCode == Models.Enum.StatusCode.NullEntity)
                {

                    return BadRequest(updatedTask.Description);
                }
                else
                {
                    return Ok(updatedTask.Data);
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to update user: {ex.Message}");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTask(int id)
        {
            try
            {
                await _taskService.DeleteTask(id);
                return Ok("User deleted successfully");


            }
            catch (Exception ex)
            {
                return BadRequest($"Failed to delete user: {ex.Message}");
            }
        }
    }



}

