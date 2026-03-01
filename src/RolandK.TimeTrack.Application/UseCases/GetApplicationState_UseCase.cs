using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.State;

namespace RolandK.TimeTrack.Application.UseCases;

using HandlerResult = TimeTrackApplicationStatePublic;

public class GetApplicationState_UseCase(TimeTrackApplicationState state)
{
    public Task<HandlerResult> GetStateAsync(CancellationToken cancellationToken)
    {
        return Task.FromResult(new TimeTrackApplicationStatePublic(
            state.ServiceStartupTimestamp != DateTimeOffset.MinValue ? state.ServiceStartupTimestamp : null,
            state.LastSuccessfulExport != DateTimeOffset.MinValue ? state.LastSuccessfulExport : null));
    }   
}