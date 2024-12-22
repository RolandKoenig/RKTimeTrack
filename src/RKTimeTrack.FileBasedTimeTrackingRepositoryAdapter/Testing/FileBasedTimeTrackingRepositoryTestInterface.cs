using RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;

namespace RKTimeTrack.FileBasedTimeTrackingRepositoryAdapter.Testing;

class FileBasedTimeTrackingRepositoryTestInterface(TimeTrackingStore store)
    : IFileBasedTimeTrackingRepositoryTestInterface
{
    public void ResetRepository()
    {
        store.Reset();
    }
}