using System.ComponentModel.DataAnnotations;
using FluentValidation;

namespace RKTimeTrack.Application.UseCases;

// ReSharper disable once ClassNeverInstantiated.Global
public record GetWeekRequest(int Year, int WeekNumber)
{
    public class Validator : AbstractValidator<GetWeekRequest>
    {
        public Validator()
        {
            RuleFor(request => request.Year)
                .GreaterThanOrEqualTo(2022);
            
            RuleFor(request => request.WeekNumber)
                .GreaterThanOrEqualTo(1)
                .LessThanOrEqualTo(53);
        }
    }
}