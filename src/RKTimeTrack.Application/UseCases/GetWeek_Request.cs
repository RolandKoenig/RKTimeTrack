using FluentValidation;
using RKTimeTrack.Application.Util;

namespace RKTimeTrack.Application.UseCases;

// ReSharper disable once ClassNeverInstantiated.Global
public record GetWeek_Request(int? Year, int? WeekNumber)
{
    public bool IsWeekProvided()
    {
        return Year.HasValue && WeekNumber.HasValue;
    }
    
    public class Validator : AbstractValidator<GetWeek_Request>
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