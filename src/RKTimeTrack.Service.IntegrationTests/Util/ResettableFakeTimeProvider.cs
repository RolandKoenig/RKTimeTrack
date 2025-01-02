using Microsoft.Extensions.Time.Testing;

namespace RKTimeTrack.Service.IntegrationTests.Util;

/// <summary>
/// Wrapper class around <see cref="FakeTimeProvider"/> to be able to
/// reset it after each IntegrationTest run
/// </summary>
public class ResettableFakeTimeProvider : TimeProvider
{
    private readonly DateTimeOffset _defaultStartTimeStamp;
    private FakeTimeProvider _fakeTimeProvider;

    public override long TimestampFrequency => _fakeTimeProvider.TimestampFrequency;
    
    public override TimeZoneInfo LocalTimeZone => _fakeTimeProvider.LocalTimeZone;
    
    public TimeSpan AutoAdvanceAmount
    {
        get => _fakeTimeProvider.AutoAdvanceAmount;
        set => _fakeTimeProvider.AutoAdvanceAmount = value;
    }
    
    public ResettableFakeTimeProvider(DateTimeOffset defaultStartTimeStamp)
    {
        _defaultStartTimeStamp = defaultStartTimeStamp;
        _fakeTimeProvider = new FakeTimeProvider(defaultStartTimeStamp);
    }
    
    public void Reset()
    {
        _fakeTimeProvider = new FakeTimeProvider(_defaultStartTimeStamp);
    }

    public void Reset(DateTimeOffset customStartTimeStamp)
    {
        _fakeTimeProvider = new FakeTimeProvider(customStartTimeStamp);
    }
    
    public override ITimer CreateTimer(TimerCallback callback, object? state, TimeSpan dueTime, TimeSpan period)
        => _fakeTimeProvider.CreateTimer(callback, state, dueTime, period);
    
    public override long GetTimestamp() 
        => _fakeTimeProvider.GetTimestamp();
    
    public override DateTimeOffset GetUtcNow() 
        => _fakeTimeProvider.GetUtcNow();
    
    public void SetUtcNow(DateTimeOffset value)
        => _fakeTimeProvider.SetUtcNow(value);

    public void Advance(TimeSpan delta)
        => _fakeTimeProvider.Advance(delta);
    
    public void SetLocalTimeZone(TimeZoneInfo localTimeZone)
        => _fakeTimeProvider.SetLocalTimeZone(localTimeZone);

    public override string ToString()
        => _fakeTimeProvider.ToString();
}