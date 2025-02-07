using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.Ports;

namespace RolandK.TimeTrack.Application.UseCases;

using HandlerResult = OneOf.OneOf<
    IReadOnlyList<TimeTrackingEntry>,
    CommonErrors.ValidationError>;

public class SearchEntriesByText_UseCase(ITimeTrackingRepository timeTrackingRepository)
{
    public async Task<HandlerResult> SearchEntriesByTextAsync(SearchEntriesByText_Request request, CancellationToken cancellationToken)
    {
        var validator = new SearchEntriesByText_Request.Validator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) { return new CommonErrors.ValidationError(validationResult.Errors); }

        if (string.IsNullOrWhiteSpace(request.SearchText))
        {
            return Array.Empty<TimeTrackingEntry>();
        }
        
        var allDaysInAscendingOrder = await timeTrackingRepository.GetAllDaysInAscendingOrderAsync(cancellationToken);
        var result = allDaysInAscendingOrder
            .Reverse()
            .SelectMany(x => x.Entries)
            .Where(actEntry => actEntry.Description.Contains(request.SearchText, StringComparison.OrdinalIgnoreCase))
            .Take(request.MaxSearchResults)
            .ToArray();
        
        return result;
    }
}