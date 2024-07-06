using FluentAssertions;
using RKTimeTrack.StaticTopicRepositoryAdapter.Util;

namespace RKTimeTrack.StaticTopicRepositoryAdapterTests.Util;

public class StaticTopicRepositoryUtilTests
{
    [Fact]
    public async Task WaitForCanceled()
    {
        // Arrange
        var cancelTokenSource = new CancellationTokenSource(TimeSpan.FromMilliseconds(200));

        // Act
        var task = StaticTopicRepositoryUtil.WaitForCancelAsync(cancelTokenSource.Token);
        await task;
        
        // Assert
        cancelTokenSource.IsCancellationRequested.Should().BeTrue();
        task.IsCompleted.Should().BeTrue();
        task.IsCompletedSuccessfully.Should().BeTrue();
    }

    [Fact]
    public async Task WaitForAlreadyCanceled()
    {
        // Arrange
        var cancelTokenSource = new CancellationTokenSource();
        await cancelTokenSource.CancelAsync();

        // Act
        var task = StaticTopicRepositoryUtil.WaitForCancelAsync(cancelTokenSource.Token);
        await task;
        
        // Assert
        cancelTokenSource.IsCancellationRequested.Should().BeTrue();
        task.IsCompleted.Should().BeTrue();
        task.IsCompletedSuccessfully.Should().BeTrue();
    }
}