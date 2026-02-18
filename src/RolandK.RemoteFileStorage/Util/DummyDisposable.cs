namespace RolandK.RemoteFileStorage.Util;

class DummyDisposable : IDisposable
{
    private readonly Action _onDisposeAction;
    
    public DummyDisposable(Action onDisposeAction)
    {
        _onDisposeAction = onDisposeAction;
    }

    public void Dispose()
    {
        _onDisposeAction.Invoke();
    }
}