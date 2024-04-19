using FluentAssertions;
using HomeworkApp.Dal.Entities;
using HomeworkApp.Dal.Models;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.IntegrationTests.Creators;
using HomeworkApp.IntegrationTests.Fakers;
using HomeworkApp.IntegrationTests.Fixtures;
using Xunit;

namespace HomeworkApp.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class TaskRepositoryTests
{
    private long parentTaskId;
    private readonly ITaskRepository _repository;

    public TaskRepositoryTests(TestFixture fixture)
    {
        _repository = fixture.TaskRepository;
    }

    private async Task SetupTasks()
    {
        int delay = 200;
        await _repository.DeleteAllTasks(CancellationToken.None);

        await Task.Delay(delay);

        var parentTask = new TaskEntityV1
        {
            ParentTaskId = null,
            Number = "T001",
            Title = "Parent Task",
            Description = "This is a parent task",
            Status = (int)TaskStatus.Created,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedByUserId = 1,
            AssignedToUserId = null,
            CompletedAt = null
        };

        var parentTaskIds = await _repository.Add(new[] { parentTask }, CancellationToken.None);
        this.parentTaskId = parentTaskIds[0];

        await Task.Delay(delay);

        var childTasks = new List<TaskEntityV1>
    {
        new TaskEntityV1
        {
            ParentTaskId = parentTaskId,
            Number = "T002",
            Title = "Child Task 1",
            Description = "This is a child task of the Parent Task",
            Status = (int)TaskStatus.WaitingToRun,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedByUserId = 1,
            AssignedToUserId = null,
            CompletedAt = null
        },
        new TaskEntityV1
        {
            ParentTaskId = parentTaskId,
            Number = "T003",
            Title = "Child Task 2",
            Description = "Another child task of the Parent Task",
            Status = (int)TaskStatus.Running,
            CreatedAt = DateTimeOffset.UtcNow,
            CreatedByUserId = 1,
            AssignedToUserId = null,
            CompletedAt = null
        }
    };

        await _repository.Add(childTasks.ToArray(), CancellationToken.None);

        await Task.Delay(delay); 
    }

    [Fact]
    public async Task Add_Task_Success()
    {
        const int count = 5;

        var tasks = TaskEntityV1Faker.Generate(count);

        var results = await _repository.Add(tasks, default);

        results.Should().HaveCount(count);
        results.Should().OnlyContain(x => x > 0);
    }
    
    [Fact]
    public async Task Get_SingleTask_Success()
    {
        var tasks = TaskEntityV1Faker.Generate();
        var taskIds = await _repository.Add(tasks, default);
        var expectedTaskId = taskIds.First();
        var expectedTask = tasks.First()
            .WithId(expectedTaskId);

        var results = await _repository.Get(new TaskGetModel()
        {
            TaskIds = new[] { expectedTaskId }
        }, default);

        results.Should().HaveCount(1);
        var task = results.Single();

        task.Should().BeEquivalentTo(expectedTask);
    }
    
    [Fact]
    public async Task AssignTask_Success()
    {
        var assigneeUserId = Create.RandomId();
        
        var tasks = TaskEntityV1Faker.Generate();
        var taskIds = await _repository.Add(tasks, default);
        var expectedTaskId = taskIds.First();
        var expectedTask = tasks.First()
            .WithId(expectedTaskId)
            .WithAssignedToUserId(assigneeUserId);
        var assign = AssignTaskModelFaker.Generate()
            .First()
            .WithTaskId(expectedTaskId)
            .WithAssignToUserId(assigneeUserId);

        await _repository.Assign(assign, default);

        var results = await _repository.Get(new TaskGetModel()
        {
            TaskIds = new[] { expectedTaskId }
        }, default);
        
        results.Should().HaveCount(1);
        var task = results.Single();
        
        expectedTask = expectedTask with {Status = assign.Status};
        task.Should().BeEquivalentTo(expectedTask);
    }

    [Fact]
    public async Task GetSubTasksInStatus_ReturnsCorrectData()
    {
        await SetupTasks();  

        long parentTaskId = this.parentTaskId; 

        TaskStatus[] statuses = {
            TaskStatus.WaitingToRun,
            TaskStatus.Running
    };

        var subTasks = await _repository.GetSubTasksInStatus(
            parentTaskId, statuses, CancellationToken.None);

        subTasks.Should().NotBeNull();
        subTasks.Should().OnlyContain(t => statuses.Contains(t.Status) && t.ParentTaskIds.Contains(parentTaskId));
    }

    [Fact]
    public async Task GetSubTasksInStatus_CorrectlyFiltersByStatusAndHierarchy()
    {
        await SetupTasks();
        var parentTaskId = 1; 
        var validStatuses = new[] { TaskStatus.WaitingToRun, TaskStatus.Created };

        var result = await _repository.GetSubTasksInStatus(parentTaskId, validStatuses, CancellationToken.None);

        if (result != null && validStatuses != null && validStatuses.Any())
        {
            result.Should().NotContain(x => x.TaskId == parentTaskId);
        }
    }

}
