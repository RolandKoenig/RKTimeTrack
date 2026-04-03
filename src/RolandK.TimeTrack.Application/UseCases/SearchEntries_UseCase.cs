using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.Ports;

namespace RolandK.TimeTrack.Application.UseCases;

using HandlerResult = OneOf.OneOf<
    IReadOnlyList<TimeTrackingEntrySearchResult>,
    CommonErrors.ValidationError>;

public class SearchEntries_UseCase(
    ITimeTrackingRepository timeTrackingRepository,
    ITopicRepository topicRepository)
{
    public async Task<HandlerResult> SearchEntriesAsync(SearchEntries_Request request, CancellationToken cancellationToken)
    {
        var validator = new SearchEntries_Request.Validator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) { return new CommonErrors.ValidationError(validationResult.Errors); }

        if (string.IsNullOrWhiteSpace(request.SearchText) &&
            request.Billed == null &&
            request.CanBeInvoiced == null)
        {
            return Array.Empty<TimeTrackingEntrySearchResult>();
        }
        
        var allTopics = await topicRepository.GetAllTopicsAsync(cancellationToken);
        var canBeInvoicedLookup = allTopics.ToDictionary(
            x => (x.Category, x.Name),
            x => x.CanBeInvoiced);

        var result = new List<TimeTrackingEntrySearchResult>(request.MaxSearchResults);
        var allDaysInAscendingOrder = await timeTrackingRepository.GetAllDaysInAscendingOrderAsync(cancellationToken);
        var allDaysInDescendingOrderQuery = allDaysInAscendingOrder.Reverse();
        foreach (var actDay in allDaysInDescendingOrderQuery)
        {
            foreach (var actEntry in actDay.Entries)
            {
                // Search by text
                if (!string.IsNullOrWhiteSpace(request.SearchText) &&
                    !actEntry.Description.Contains(request.SearchText, StringComparison.OrdinalIgnoreCase))
                {
                    continue;
                }

                // Search by 'billed' property
                if (request.Billed.HasValue &&
                    actEntry.Billed != request.Billed.Value)
                {
                    continue;
                }

                // Search by 'can be invoiced' property
                if (request.CanBeInvoiced.HasValue)
                {
                    canBeInvoicedLookup.TryGetValue((actEntry.Topic.Category, actEntry.Topic.Name), out var canBeInvoiced);
                    if (canBeInvoiced != request.CanBeInvoiced.Value)
                    {
                        continue;
                    }
                }
                
                result.Add(new TimeTrackingEntrySearchResult(
                    actDay.Date,
                    actEntry.Topic,
                    actEntry.EffortInHours,
                    actEntry.EffortBilled,
                    actEntry.BillingMultiplier,
                    actEntry.Billed,
                    actEntry.Type,
                    actEntry.Description));
                if (result.Count >= request.MaxSearchResults) { break; }
            }
            if (result.Count >= request.MaxSearchResults) { break; }
        }
        
        return result;
    }
}