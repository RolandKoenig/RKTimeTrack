using System.ComponentModel.DataAnnotations;
using FluentValidation;
using ValidationResult = FluentValidation.Results.ValidationResult;

namespace RKTimeTrack.Application.UseCases;

public record GetWeekRequest(
    [Range(1, int.MaxValue)] int Year,
    [Range(1, 52)] int WeekNumber)
{
    public class Validator : AbstractValidator<GetWeekRequest>
    {
        public override ValidationResult Validate(ValidationContext<GetWeekRequest> context)
        {
            RuleFor(request => request.Year)
                .GreaterThanOrEqualTo(2022);
            
            RuleFor(request => request.WeekNumber)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(52);
            
            return base.Validate(context);
        }
    }
}