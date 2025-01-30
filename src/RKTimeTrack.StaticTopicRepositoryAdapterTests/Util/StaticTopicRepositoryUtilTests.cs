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
        Assert.True(cancelTokenSource.IsCancellationRequested);
        Assert.True(task.IsCompleted);
        Assert.True(task.IsCompletedSuccessfully);
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
        Assert.True(cancelTokenSource.IsCancellationRequested);
        Assert.True(task.IsCompleted);
        Assert.True(task.IsCompletedSuccessfully);
    }
}