namespace RKTimeTrack.StaticTopicRepositoryAdapter.Util;

internal static class StaticTopicRepositoryUtil
{
    public static Task WaitForCancelAsync(CancellationToken cancellationToken)
    {
        var tcs = new TaskCompletionSource<bool>();
        cancellationToken.Register(_ => tcs.SetResult(true), null);
        return tcs.Task;
    }
}