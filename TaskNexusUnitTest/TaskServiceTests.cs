using Moq;
using TaskNexus.DAL.Interfaces;
using TaskNexus.Models.Entity;
using TaskNexus.Models.Enum;
using TaskNexus.Service.ImplementationsService;

namespace TaskNexusUnitTest
{
    public class TaskServiceTests
    {
        private readonly TaskService _taskService;
        private readonly Mock<ITaskRepository> _taskRepositoryMock;

        public TaskServiceTests()
        {
            _taskRepositoryMock = new Mock<ITaskRepository>();
            _taskService = new TaskService(_taskRepositoryMock.Object);
        }

        #region Createtask

        [Fact]
        public async Task CreateTask_ValidInput_ReturnsTrue()
        {
            // Arrange
            var entity = new Task_Entity();
            string userId = "testUserId";

            // Act
            var result = await _taskService.CreateTask(entity, userId);

            // Assert
            Assert.True(result.Data);
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal("Good job", result.Description);
        }

        [Fact]
        public async Task CreateTask_NullEntity_ReturnsFalseWithStatusCodeNullEntity()
        {
            // Arrange
            Task_Entity entity = null;
            string userId = "testUserId";

            // Act
            var result = await _taskService.CreateTask(entity, userId);

            // Assert
            Assert.False(result.Data);
            Assert.Equal(StatusCode.NullEntity, result.StatusCode);
            Assert.Equal("task = null", result.Description);
        }

        [Fact]
        public async Task CreateTask_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            var entity = new Task_Entity();
            string userId = "testUserId";

            _taskRepositoryMock.Setup(repo => repo.Create(It.IsAny<Task_Entity>())).ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _taskService.CreateTask(entity, userId);

            // Assert
            Assert.False(result.Data);
            Assert.Equal(StatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("[CreateTask] : Test Exception", result.Description);
        }
        #endregion

        #region Deletetask
        [Fact]
        public async Task DeleteTask_ValidId_ReturnsTrue()
        {
            // Arrange
            int taskId = 1;

            _taskRepositoryMock.Setup(repo => repo.Get(taskId)).ReturnsAsync(new Task_Entity { Id = taskId });

            // Act
            var result = await _taskService.DeleteTask(taskId);

            // Assert
            Assert.True(result.Data);
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal("Good delete task", result.Description);
        }

        [Fact]
        public async Task DeleteTask_InvalidId_ReturnsFalseWithStatusCodeNullEntity()
        {
            // Arrange
            int taskId = 1;

            _taskRepositoryMock.Setup(repo => repo.Get(taskId)).ReturnsAsync((Task_Entity)null);

            // Act
            var result = await _taskService.DeleteTask(taskId);

            // Assert
            Assert.False(result.Data);
            Assert.Equal(StatusCode.NullEntity, result.StatusCode);
            Assert.Equal("deletetask = null entity", result.Description);
        }

        [Fact]
        public async Task DeleteTask_ExceptionThrown_ReturnsInternalServerError()
        {
            // Arrange
            int taskId = 1;

            _taskRepositoryMock.Setup(repo => repo.Get(taskId)).ThrowsAsync(new Exception("Test Exception"));

            // Act
            var result = await _taskService.DeleteTask(taskId);

            // Assert
            Assert.False(result.Data);
            Assert.Equal(StatusCode.InternalServerError, result.StatusCode);
            Assert.Contains("[DeleteTask] : Test Exception", result.Description);
        }
        #endregion

        #region Gettasks
        [Fact]
        public async Task GetTasks_ValidUserId_ReturnsTasks()
        {
            // Arrange
            string userId = "testUserId";
            var tasks = new List<Task_Entity> { new Task_Entity { AssignedToId = userId } };

            _taskRepositoryMock.Setup(repo => repo.Select(userId)).ReturnsAsync(tasks);

            // Act
            var result = await _taskService.GetTasks(userId);

            // Assert
            Assert.NotNull(result.Data);
            Assert.NotEmpty(result.Data);
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal("Good job", result.Description);
        }

    

        [Fact]
        public async Task UpdateTask_ValidId_ReturnsUpdatedTask()
        {
            // Arrange
            int taskId = 1;
            var entity = new Task_Entity { Id = taskId };

            _taskRepositoryMock.Setup(repo => repo.Get(taskId)).ReturnsAsync(new Task_Entity { Id = taskId });

            // Act
            var result = await _taskService.UpdateTask(taskId, entity);

            // Assert
            Assert.NotNull(result.Data);
            Assert.Equal(taskId, result.Data.Id); // Verify if returned task has the same ID as the one provided
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal("Good job", result.Description);
        }
        #endregion

        #region Updatetask
        [Fact]
        public async Task UpdateTask_InvalidId_ReturnsNullEntity()
        {
            // Arrange
            int taskId = 1;
            var entity = new Task_Entity { Id = taskId };

            _taskRepositoryMock.Setup(repo => repo.Get(taskId)).ReturnsAsync((Task_Entity)null);

            // Act
            var result = await _taskService.UpdateTask(taskId, entity);

            // Assert
            Assert.Null(result.Data);
            Assert.Equal(StatusCode.NullEntity, result.StatusCode);
            Assert.Equal("Updade task no entity", result.Description);
        }
        #endregion



    }
}