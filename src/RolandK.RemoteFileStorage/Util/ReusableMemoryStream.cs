using System.Collections.Concurrent;

namespace RolandK.RemoteFileStorage.Util;

class ReusableMemoryStreams
{
    private readonly ConcurrentStack<MemoryStream> _memoryStreams = new();

    public int Count => _memoryStreams.Count;

    public static ReusableMemoryStreams Current { get; }

    static ReusableMemoryStreams()
    {
        Current = new ReusableMemoryStreams();
    }

    public IDisposable UseMemoryStream(out MemoryStream memoryStream, int requiredCapacity = 128)
    {
        memoryStream = this.TakeMemoryStream(requiredCapacity);

        var cachedMemoryStream = memoryStream;
        return new DummyDisposable(() => this.ReRegisterMemoryStream(cachedMemoryStream));
    }

    public MemoryStream TakeMemoryStream(int requiredCapacity = 128)
    {
        if (!_memoryStreams.TryPop(out var result))
        {
            result = new MemoryStream(requiredCapacity);
        }
        else
        {
            if (result.Capacity < requiredCapacity) { result.Capacity = requiredCapacity; }
        }
        return result;
    }

    public void ReRegisterMemoryStream(MemoryStream memoryStream)
    {
        memoryStream.SetLength(0);
        _memoryStreams.Push(memoryStream);
    }

    public void Clear()
    {
        _memoryStreams.Clear();
    }
}