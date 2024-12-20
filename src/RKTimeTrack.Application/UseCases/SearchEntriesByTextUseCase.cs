using RKTimeTrack.Application.Models;
using RKTimeTrack.Application.Ports;

namespace RKTimeTrack.Application.UseCases;

using HandlerResult = OneOf.OneOf<
    IReadOnlyList<TimeTrackingEntry>,
    CommonErrors.ValidationError>;

public class SearchEntriesByTextUseCase(ITimeTrackingRepository timeTrackingRepository)
{
    public async Task<HandlerResult> SearchEntriesByTextAsync(SearchEntriesByTextRequest request, CancellationToken cancellationToken)
    {
        // Validate
        var validator = new SearchEntriesByTextRequest.Validator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) { return new CommonErrors.ValidationError(validationResult.Errors); }

        // Search logic
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