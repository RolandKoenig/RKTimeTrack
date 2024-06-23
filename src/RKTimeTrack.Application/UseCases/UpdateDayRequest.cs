using FluentValidation;
using RKTimeTrack.Application.Models;

namespace RKTimeTrack.Application.UseCases;

public record UpdateDayRequest(DateOnly Date, TimeTrackingDayType Type, IReadOnlyList<TimeTrackingRow> Entries)
{
    public class Validator : AbstractValidator<UpdateDayRequest>
    {
        public Validator()
        {
            
        }
    }
}