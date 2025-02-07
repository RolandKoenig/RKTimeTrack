using RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;

namespace RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Testing;

class FileBasedTimeTrackingRepositoryTestInterface(TimeTrackingStore store)
    : IFileBasedTimeTrackingRepositoryTestInterface
{
    public void ResetRepository()
    {
        store.Reset();
    }
}