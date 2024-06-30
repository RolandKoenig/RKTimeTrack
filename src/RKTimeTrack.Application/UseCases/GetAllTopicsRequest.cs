using FluentValidation;

namespace RKTimeTrack.Application.UseCases;

public record class GetAllTopicsRequest
{
    public class Validator : AbstractValidator<GetAllTopicsRequest>
    {
        public Validator()
        {
            // Nothing to validate
        }
    }
}