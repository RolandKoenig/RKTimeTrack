using System.Text.Json;
using Microsoft.Extensions.Logging;
using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Ports;
using RKTimeTrack.StaticTopicRepositoryAdapter.Util;

namespace RKTimeTrack.StaticTopicRepositoryAdapter;

class StaticTopicRepository : ITopicRepository
{
    private readonly ILogger _logger;
    private readonly StaticTopicRepositoryOptions _options;
    private readonly Lazy<Task<IReadOnlyCollection<TimeTrackingTopic>>> _topics;

    public StaticTopicRepository(
        ILogger<StaticTopicRepository> logger,
        StaticTopicRepositoryOptions options)
    {
        _logger = logger;
        _options = options;
        _topics = new Lazy<Task<IReadOnlyCollection<TimeTrackingTopic>>>(ReadOrGenerateAllTopicsAsync);
    }
    
    private async Task<IReadOnlyCollection<TimeTrackingTopic>> ReadOrGenerateAllTopicsAsync()
    {
        if (_options.GenerateTestData) { return TestDataGenerator.CreateTestData(); }
        
        if (!string.IsNullOrEmpty(_options.SourceFilePath))
        {
            try
            {
                var jsonOptions = new JsonSerializerOptions(JsonSerializerDefaults.Web);

                await using var inStream = File.OpenRead(_options.SourceFilePath);

                var result = await JsonSerializer
                    .DeserializeAsync<IReadOnlyCollection<TimeTrackingTopic>>(inStream, jsonOptions);
                if (result == null)
                {
                    _logger.LogError(
                        $"Unable to read from json file {_options.SourceFilePath}: Serializer returned null!");
                    result = [];
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    $"Unable to read from json file {_options.SourceFilePath}");
            }
        }

        _logger.LogWarning($"{nameof(_options.SourceFilePath)} option not specified!");
        return [];
    }
    
    public async Task<IReadOnlyCollection<TimeTrackingTopic>> GetAllTopicsAsync(CancellationToken cancellationToken)
    {
        await Task.WhenAny(
            StaticTopicRepositoryUtil.WaitForCancelAsync(cancellationToken),
            _topics.Value);
        if (!_topics.Value.IsCompleted)
        {
            throw new TimeoutException();
        }
        
        return await _topics.Value;
    }
}