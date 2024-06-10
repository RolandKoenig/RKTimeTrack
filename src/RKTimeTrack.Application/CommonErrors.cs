using FluentValidation.Results;

namespace RKTimeTrack.Application;

public static class CommonErrors
{
    public record struct ValidationError(IReadOnlyCollection<ValidationFailure> errors);
}