using FluentValidation;

namespace RKTimeTrack.Application.UseCases;

public record GetAllTopics_Request
{
    public class Validator : AbstractValidator<GetAllTopics_Request>
    {
        
    }
}