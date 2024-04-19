using HomeworkApp.Bll.Services;
using HomeworkApp.Dal.Entities;
using HomeworkApp.Dal.Repositories.Interfaces;
using Microsoft.Extensions.Caching.Distributed;
using Moq;
using Xunit;

namespace HomeworkApp.IntegrationTests.RepositoryTests
{
    public class TaskServiceTests
    {
        private readonly Mock<ITaskCommentRepository> _mockTaskCommentRepository;
        private readonly Mock<IDistributedCacheService> _mockDistributedCache;
        private readonly Mock<ITaskRepository> _mockTaskRepository;
        private readonly Mock<ITaskLogRepository> _mockTaskLogRepository;
        private readonly Mock<ITakenTaskRepository> _mockTakenTaskRepository; 
        private readonly TaskService _taskService;

        public TaskServiceTests()
        {
            _mockTaskCommentRepository = new Mock<ITaskCommentRepository>();
            _mockTaskRepository = new Mock<ITaskRepository>();
            _mockTaskLogRepository = new Mock<ITaskLogRepository>();
            _mockTakenTaskRepository = new Mock<ITakenTaskRepository>();
            _mockDistributedCache = new Mock<IDistributedCacheService>();

            _taskService = new TaskService(
                _mockTaskRepository.Object,
                _mockTaskLogRepository.Object,
                _mockTakenTaskRepository.Object,
                _mockTaskCommentRepository.Object,
                _mockDistributedCache.Object 
            );
        }



        [Fact]
        public async Task GetComments_ShouldReturnFromCache_IfCacheExists()
        {

            var taskId = 1L;
            var cacheKey = $"comments_{taskId}";
            var cachedData = "[{\"TaskId\":1,\"Comment\":\"Test comment\",\"IsDeleted\":false,\"At\":\"2022-01-01T00:00:00+00:00\"}]";
            _mockDistributedCache.Setup(x => x.GetStringAsync(cacheKey, It.IsAny<CancellationToken>()))
                .ReturnsAsync(cachedData);

            var result = await _taskService.GetComments(taskId, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("Test comment", result[0].Comment);
            _mockTaskCommentRepository.Verify(x => x.Get(It.IsAny<TaskCommentGetModel>(), It.IsAny<CancellationToken>()), Times.Never);
        }

        [Fact]
        public async Task GetComments_ShouldFetchFromDatabase_IfCacheNotExists()
        {
            var taskId = 1L;
            var cacheKey = $"comments_{taskId}";
            _mockDistributedCache.Setup(x => x.GetStringAsync(cacheKey, It.IsAny<CancellationToken>()))
                .ReturnsAsync((string)null);
            var comments = new TaskCommentEntityV1[]
            {
            new TaskCommentEntityV1 { TaskId = taskId, Message = "New comment", At = DateTimeOffset.UtcNow }
            };
            _mockTaskCommentRepository.Setup(x => x.Get(It.IsAny<TaskCommentGetModel>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(comments);

            var result = await _taskService.GetComments(taskId, CancellationToken.None);

            Assert.Single(result);
            Assert.Equal("New comment", result[0].Comment);
            _mockDistributedCache.Verify(x => x.SetStringAsync(cacheKey, It.IsAny<string>(), It.IsAny<DistributedCacheEntryOptions>(), It.IsAny<CancellationToken>()), Times.Once);
        }
    }
}
