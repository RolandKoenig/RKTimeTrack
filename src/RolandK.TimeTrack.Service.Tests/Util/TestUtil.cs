namespace RolandK.TimeTrack.Service.Tests.Util;

public static class TestUtil
{
    public static async Task TryXTimesAsync(Func<Task> asyncAction, int times, TimeSpan delay)
    {
        for (var loop = 0; loop < times; loop++)
        {
            try
            {
                await asyncAction();
                break;
            }
            catch (Exception)
            {
                if (loop >= times - 1)
                {
                    throw;
                }
            }
            
            await Task.Delay(delay);
        }
    }
}