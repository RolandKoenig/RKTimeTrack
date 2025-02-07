using FluentValidation;
using RolandK.TimeTrack.Application.Util;

namespace RolandK.TimeTrack.Application.UseCases;

// ReSharper disable once ClassNeverInstantiated.Global
public record GetWeek_Request(int Year, int WeekNumber)
{
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