using FluentValidation;

namespace RKTimeTrack.Application.UseCases;

public record GetAllTopicsRequest
{
    public class Validator : AbstractValidator<GetAllTopicsRequest>
    {
        
    }
}