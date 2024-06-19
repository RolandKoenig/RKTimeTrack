using FluentValidation;

namespace RKTimeTrack.Application.UseCases;

// ReSharper disable once ClassNeverInstantiated.Global
public record GetYearMetadataRequest(int Year)
{
    public class Validator : AbstractValidator<GetYearMetadataRequest>
    {
        public Validator()
        {
            RuleFor(request => request.Year)
                .GreaterThanOrEqualTo(2022);
        }
    }
}