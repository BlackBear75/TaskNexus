using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskNexus.DAL.Interfaces;
using TaskNexus.DAL.Repository;
using TaskNexus.Models.ApplicationUser;
using TaskNexus.Models.Entity;
using TaskNexus.Models.Enum;
using TaskNexus.Service.ImplementationsService;
using TaskNexus.Service.InterfaceService;

namespace TaskNexusUnitTest
{
    public class TaskFilterTest
    {
        private readonly TaskFilterService _taskfilterService;
        private readonly Mock<ITaskFilterRepository> _taskFilterMock;

        public TaskFilterTest()
        {
            _taskFilterMock = new Mock<ITaskFilterRepository>();
            _taskfilterService = new TaskFilterService(_taskFilterMock.Object);
        }

        [Fact]
        public async Task TaskFilterExecutionStatus_ReturnsData_Successfully()
        {
            // Arrange
         
            var expectedTasks = new List<Task_Entity>() { /* your expected tasks */ };
            _taskFilterMock.Setup(repo => repo.TaskExecutionStatus(It.IsAny<string>())).ReturnsAsync(expectedTasks);

            // Act
            var result = await _taskfilterService.TaskExecutionStatus("userid");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal("Good job", result.Description);
            Assert.Equal(expectedTasks, result.Data);
        }

        [Fact]
        public async Task TaskNearestDeadline_ReturnsData_Successfully()
        {
            // Arrange
            var expectedTask = new Task_Entity(); // your expected task


            _taskFilterMock.Setup(repo => repo.TaskNearestDeadline(It.IsAny<string>())).ReturnsAsync(expectedTask);

           

            // Act
            var result = await _taskfilterService.TaskNearestDeadline("userid");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal("Good job", result.Description);
            Assert.Equal(expectedTask, result.Data);
        }

        [Fact]
        public async Task TaskNearestDeadline_NullResponse_ReturnsNullEntity()
        {
            // Arrange

            _taskFilterMock.Setup(repo => repo.TaskNearestDeadline(It.IsAny<string>())).ReturnsAsync((Task_Entity)null);


            // Act
            var result = await _taskfilterService.TaskNearestDeadline("userid");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.NullEntity, result.StatusCode);
            Assert.Equal("TaskNearestDeadline = null entity", result.Description);
            Assert.Null(result.Data);
        }


        [Fact]
        public async Task TaskNewest_ReturnsData_Successfully()
        {
            // Arrange
            var expectedTasks = new List<Task_Entity>(); // your expected tasks
         
            _taskFilterMock.Setup(repo => repo.TaskNewest(It.IsAny<string>())).ReturnsAsync(expectedTasks);

         
            // Act
            var result = await _taskfilterService.TaskNewest("userid");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal("Good job", result.Description);
            Assert.Equal(expectedTasks, result.Data);
        }

        [Fact]
        public async Task TaskNewest_NullResponse_ReturnsNullEntity()
        {
            // Arrange

            _taskFilterMock.Setup(repo => repo.TaskNewest(It.IsAny<string>())).ReturnsAsync((List<Task_Entity>)null);



            // Act
            var result = await _taskfilterService.TaskNewest("userid");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.NullEntity, result.StatusCode);
            Assert.Equal("TaskNewest no find tasks", result.Description);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task TaskOldest_ReturnsData_Successfully()
        {
            // Arrange
            var expectedTasks = new List<Task_Entity>(); // your expected tasks

            _taskFilterMock.Setup(repo => repo.TaskOldest(It.IsAny<string>())).ReturnsAsync(expectedTasks);

           
            // Act
            var result = await _taskfilterService.TaskOldest("userid");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal("Good job", result.Description);
            Assert.Equal(expectedTasks, result.Data);
        }

        [Fact]
        public async Task TaskOldest_NullResponse_ReturnsNullEntity()
        {
            // Arrange
            _taskFilterMock.Setup(repo => repo.TaskOldest(It.IsAny<string>())).ReturnsAsync((List<Task_Entity>)null);

         

            // Act
            var result = await _taskfilterService.TaskOldest("userid");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.NullEntity, result.StatusCode);
            Assert.Equal("TaskOldest no find tasks", result.Description);
            Assert.Null(result.Data);
        }
        [Fact]
        public async Task TaskPriority_ReturnsData_Successfully()
        {
            // Arrange
            var expectedTasks = new List<Task_Entity>(); // your expected tasks

            _taskFilterMock.Setup(repo => repo.TaskPriority(It.IsAny<string>())).ReturnsAsync(expectedTasks);

           

            // Act
            var result = await _taskfilterService.TaskPriority("userid");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal("Good job", result.Description);
            Assert.Equal(expectedTasks, result.Data);
        }

        [Fact]
        public async Task TaskPriority_NullResponse_ReturnsNullEntity()
        {
            // Arrange
            _taskFilterMock.Setup(repo => repo.TaskPriority(It.IsAny<string>())).ReturnsAsync((List<Task_Entity>)null);



            // Act
            var result = await _taskfilterService.TaskPriority("userid");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.NullEntity, result.StatusCode);
            Assert.Equal("TaskPriority no find tasks", result.Description);
            Assert.Null(result.Data);
        }

        [Fact]
        public async Task GetEvaluationUser_WithTasks_ReturnsEvaluationUser_Successfully()
        {
            // Arrange
            var expectedTasks = new List<Task_Entity> { new Task_Entity { Status = TaskNexus.Models.Enum.TaskStatus.Completed } }; // assuming user has at least one completed task
            _taskFilterMock.Setup(repo => repo.UserTasks(It.IsAny<string>())).ReturnsAsync(expectedTasks);

            var expectedTaskCompleted = expectedTasks.Count(t => t.Status == TaskNexus.Models.Enum.TaskStatus.Completed);
            var mockEvaluationUser = new EvaluationUser(); // expected evaluation user
            _taskFilterMock.Setup(repo => repo.GetEvaluationUser(expectedTasks.Count, expectedTaskCompleted)).ReturnsAsync(mockEvaluationUser);

            var service = new TaskFilterService(_taskFilterMock.Object);

            // Act
            var result = await service.GetEvaluationUser("userid");

            // Assert
            Assert.NotNull(result);
            Assert.Equal(StatusCode.OK, result.StatusCode);
            Assert.Equal("Good job", result.Description);
            Assert.Equal(mockEvaluationUser, result.Data);
        }

        [Fact]
            public async Task GetEvaluationUser_WithoutTasks_ReturnsNullEntity()
            {
                // Arrange
                var expectedTasks = new List<Task_Entity>(); // assuming user has no tasks
                _taskFilterMock.Setup(repo => repo.UserTasks(It.IsAny<string>())).ReturnsAsync(expectedTasks);

                var service = new TaskFilterService(_taskFilterMock.Object);

                // Act
                var result = await service.GetEvaluationUser("userid");

                // Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCode.NullEntity, result.StatusCode);
                Assert.Equal("List null", result.Description);
                Assert.Null(result.Data);
            }

            [Fact]
            public async Task GetEvaluationUser_ExceptionThrown_ReturnsInternalServerError()
            {
                // Arrange
                _taskFilterMock.Setup(repo => repo.UserTasks(It.IsAny<string>())).ThrowsAsync(new Exception("Test exception"));

                var service = new TaskFilterService(_taskFilterMock.Object);

                // Act
                var result = await service.GetEvaluationUser("userid");

                // Assert
                Assert.NotNull(result);
                Assert.Equal(StatusCode.InternalServerError, result.StatusCode);
                Assert.Equal("[GetEvaluationUser] : Test exception", result.Description);
                Assert.Null(result.Data);
            }

        }
}
