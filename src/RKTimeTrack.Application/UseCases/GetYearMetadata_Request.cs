using FluentValidation;

namespace RKTimeTrack.Application.UseCases;

// ReSharper disable once ClassNeverInstantiated.Global
public record GetYearMetadata_Request(int Year)
{
    public class Validator : AbstractValidator<GetYearMetadata_Request>
    {
        public Validator()
        {
            RuleFor(request => request.Year)
                .GreaterThanOrEqualTo(2022);
        }
    }
}