using RKTimeTrack.Application.Models;
using RKTimeTrack.StaticTopicRepositoryAdapter;

namespace RKTimeTrack.StaticTopicRepositoryAdapterTests;

public class StaticTopicRepositoryTests
{
    [Fact]
    public async Task InitializeFromFile()
    {
        // Arrange
        var fileName = $"TestData-{Guid.NewGuid()}.json";
        await File.WriteAllTextAsync(
            fileName,
            """
            [
                {
                    "category": "cat1",
                    "name": "name1"
                },
                {
                    "category": "cat1",
                    "name": "name2",
                    "budget": 40
                }
            ]
            """
            );
        try
        {
            var options = new StaticTopicRepositoryOptions()
            {
                GenerateTestData = false,
                SourceFilePath = fileName
            };
        
            // Act
            var staticTopicResository = new StaticTopicRepository(
                new DummyLogger<StaticTopicRepository>(),
                options);
            var allTopics = await staticTopicResository.GetAllTopicsAsync(CancellationToken.None);
        
            // Assert
            Assert.NotEmpty(allTopics);
            Assert.Equal(2, allTopics.Count);

            Assert.Equal("cat1", allTopics[0].Category);
            Assert.Equal("name1", allTopics[0].Name);
            Assert.Null(allTopics[0].Budget);
            Assert.Equal("cat1", allTopics[1].Category);
            Assert.Equal("name2", allTopics[1].Name);
            Assert.Equal(new TimeTrackingBudget(40), allTopics[1].Budget);
        }
        finally
        {
            File.Delete(fileName);
        }
    }
    
    [Fact]
    public async Task InitializeFromTestData()
    {
        // Arrange
        var options = new StaticTopicRepositoryOptions()
        {
            GenerateTestData = true,
        };
        
        // Act
        var staticTopicResository = new StaticTopicRepository(
            new DummyLogger<StaticTopicRepository>(),
            options);
        var allTopics = await staticTopicResository.GetAllTopicsAsync(CancellationToken.None);
        
        // Assert
        Assert.NotEmpty(allTopics);
    }
}