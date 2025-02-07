using FluentValidation.Results;

namespace RolandK.TimeTrack.Application;

public static class CommonErrors
{
    public record struct ValidationError(IReadOnlyCollection<ValidationFailure> errors);
}