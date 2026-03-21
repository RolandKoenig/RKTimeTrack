using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.Ports;

namespace RolandK.TimeTrack.Application.UseCases;

using HandlerResult = OneOf.OneOf<
    IReadOnlyList<TimeTrackingEntry>,
    CommonErrors.ValidationError>;

public class SearchEntriesByText_UseCase(
    ITimeTrackingRepository timeTrackingRepository,
    ITopicRepository topicRepository)
{
    public async Task<HandlerResult> SearchEntriesByTextAsync(SearchEntriesByText_Request request, CancellationToken cancellationToken)
    {
        var validator = new SearchEntriesByText_Request.Validator();
        var validationResult = await validator.ValidateAsync(request, cancellationToken);
        if (!validationResult.IsValid) { return new CommonErrors.ValidationError(validationResult.Errors); }

        if (string.IsNullOrWhiteSpace(request.SearchText) &&
            request.Billed == null &&
            request.CanBeInvoiced == null)
        {
            return Array.Empty<TimeTrackingEntry>();
        }

        var allTopics = await topicRepository.GetAllTopicsAsync(cancellationToken);
        var canBeInvoicedLookup = allTopics.ToDictionary(
            x => (x.Category, x.Name),
            x => x.CanBeInvoiced);
        
        var allDaysInAscendingOrder = await timeTrackingRepository.GetAllDaysInAscendingOrderAsync(cancellationToken);
        var result = allDaysInAscendingOrder
            .Reverse()
            .SelectMany(x => x.Entries)
            .Where(actEntry =>
            {
                // Search by text
                if (!string.IsNullOrWhiteSpace(request.SearchText) &&
                    !actEntry.Description.Contains(request.SearchText, StringComparison.OrdinalIgnoreCase))
                {
                    return false;
                }

                // Search by 'billed' property
                if (request.Billed.HasValue &&
                    actEntry.Billed != request.Billed.Value)
                {
                    return false;
                }

                // Search by 'can be invoiced' property
                if (request.CanBeInvoiced.HasValue)
                {
                    canBeInvoicedLookup.TryGetValue((actEntry.Topic.Category, actEntry.Topic.Name), out var canBeInvoiced);
                    if (canBeInvoiced != request.CanBeInvoiced.Value)
                    {
                        return false;
                    }
                }

                return true;
            })
            .Take(request.MaxSearchResults)
            .ToArray();
        
        return result;
    }
}