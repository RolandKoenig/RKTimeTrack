using FluentValidation;
using RKTimeTrack.Application.Util;

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
                .GreaterThanOrEqualTo(GermanCalendarWeekUtil.CALENDAR_WEEK_MIN)
                .LessThanOrEqualTo(GermanCalendarWeekUtil.CALENDAR_WEEK_MAX);
        }
    }
}