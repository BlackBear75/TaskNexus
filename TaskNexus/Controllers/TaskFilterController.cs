using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TaskNexus.Models.ApplicationUser;
using TaskNexus.Service.ImplementationsService;
using TaskNexus.Service.InterfaceService;

namespace TaskNexus.Controllers
{
    [Route("/api/[controller]")]
    [ApiController]
    [Authorize]
    public class TaskFilterController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ITaskFilterService _taskfilterService;
        public TaskFilterController(UserManager<ApplicationUser> userManager,ITaskFilterService taskFilterService)
        {
            _userManager = userManager;
            _taskfilterService = taskFilterService;
            
        }


        [HttpGet]
        [Route("TaskExecutionStatus")]

        public async Task<IActionResult> TaskExecutionStatus()
        {
            var user = await _userManager.GetUserAsync(User);
            var tasks = await _taskfilterService.TaskExecutionStatus(user.Id);
            if (tasks.StatusCode != Models.Enum.StatusCode.NullEntity)
            {
                return Ok(tasks);
            }
            else
            {
                return BadRequest(tasks.Description);
            }
        }

        [HttpGet]
        [Route("TaskNearestDeadline")]

        public async Task<IActionResult> TaskNearestDeadline()
        {
            var user = await _userManager.GetUserAsync(User);
            var tasks = await _taskfilterService.TaskNearestDeadline(user.Id);
            if (tasks.StatusCode != Models.Enum.StatusCode.NullEntity)
            {
                return Ok(tasks);
            }
            else
            {
                return BadRequest(tasks.Description);
            }
        }

        [HttpGet]
        [Route("TaskNewest")]

        public async Task<IActionResult> TaskNewest()
        {
            var user = await _userManager.GetUserAsync(User);
            var tasks = await _taskfilterService.TaskNewest(user.Id);
            if (tasks.StatusCode != Models.Enum.StatusCode.NullEntity)
            {
                return Ok(tasks);
            }
            else
            {
                return BadRequest(tasks.Description);
            }
        }

        [HttpGet]
        [Route("TaskOldest")]

        public async Task<IActionResult> TaskOldest()
        {
            var user = await _userManager.GetUserAsync(User);
            var tasks = await _taskfilterService.TaskOldest(user.Id);
            if (tasks.StatusCode != Models.Enum.StatusCode.NullEntity)
            {
                return Ok(tasks);
            }
            else
            {
                return BadRequest(tasks.Description);
            }
        }

        [HttpGet]
        [Route("TaskPriority")]

        public async Task<IActionResult> TaskPriority()
        {
            var user = await _userManager.GetUserAsync(User);
            var tasks = await _taskfilterService.TaskPriority(user.Id);
            if (tasks.StatusCode != Models.Enum.StatusCode.NullEntity)
            {
                return Ok(tasks);
            }
            else
            {
                return BadRequest(tasks.Description);
            }
        }




    }
}
