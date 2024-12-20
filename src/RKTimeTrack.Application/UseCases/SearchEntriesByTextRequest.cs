using FluentValidation;

namespace RKTimeTrack.Application.UseCases;

public record SearchEntriesByTextRequest(
    string SearchText,
    int MaxSearchResults = 20)
{
    public class Validator : AbstractValidator<SearchEntriesByTextRequest>
    {
        public Validator()
        {
            this
                .RuleFor(x => x.MaxSearchResults)
                .GreaterThanOrEqualTo(1);
        }
    }
}