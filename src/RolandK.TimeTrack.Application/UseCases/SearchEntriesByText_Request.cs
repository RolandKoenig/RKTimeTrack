using FluentValidation;

namespace RolandK.TimeTrack.Application.UseCases;

public record SearchEntriesByText_Request(
    string SearchText,
    bool? Billed = null,
    bool? CanBeInvoiced = null,
    int MaxSearchResults = 20)
{
    public class Validator : AbstractValidator<SearchEntriesByText_Request>
    {
        public Validator()
        {
            this
                .RuleFor(x => x.MaxSearchResults)
                .GreaterThanOrEqualTo(1);
        }
    }
}