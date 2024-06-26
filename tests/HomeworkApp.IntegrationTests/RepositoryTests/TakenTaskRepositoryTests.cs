using FluentAssertions;
using HomeworkApp.Dal.Repositories.Interfaces;
using HomeworkApp.IntegrationTests.Creators;
using HomeworkApp.IntegrationTests.Fakers;
using HomeworkApp.IntegrationTests.Fixtures;
using Xunit;

namespace HomeworkApp.IntegrationTests.RepositoryTests;

[Collection(nameof(TestFixture))]
public class TakenTaskRepositoryTests
{
    private readonly ITakenTaskRepository _repository;

    public TakenTaskRepositoryTests(TestFixture fixture)
    {
        _repository = fixture.TakenTaskRepository;
    }

    [Fact]
    public async Task Add_TakenTask_Success()
    {
        var takenTask = TakenTaskModelFaker.Generate()
            .First();

        await _repository.Add(takenTask, default);
        var expiresAt = await _repository.GetExpireIfExists(takenTask.TaskId, default);
        
        expiresAt.Should().NotBeNull();
    }
    
    [Fact]
    public async Task Get_TakenTask_Success()
    {
        var taskId = Create.RandomId();
        
        var takenTask = TakenTaskModelFaker.Generate()
            .First()
            .WithTaskId(taskId);
        await _repository.Add(takenTask, default);

        var result = await _repository.Get(taskId, default);

        result.Should().NotBeNull();
        result.Should().BeEquivalentTo(takenTask);
    }
    
    [Fact]
    public async Task Get_TakenAt_Success()
    {
        var taskId = Create.RandomId();
        
        var takenTask = TakenTaskModelFaker.Generate()
            .First()
            .WithTaskId(taskId);
        await _repository.Add(takenTask, default);

        var result = await _repository.GetTakenAt(taskId, default);

        result.Should().NotBeNull();
        result.Should().BeCloseTo(takenTask.AssignedAt, TimeSpan.FromSeconds(1));
    }
    
    [Fact]
    public async Task Delete_TakenTask_Success()
    {
        var takenTask = TakenTaskModelFaker.Generate()
            .First();
        
        await _repository.Delete(takenTask.TaskId, default);
    }
}