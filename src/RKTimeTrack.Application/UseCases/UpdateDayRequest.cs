using FluentValidation;
using RKTimeTrack.Application.Models;

namespace RKTimeTrack.Application.UseCases;

// ReSharper disable once ClassNeverInstantiated.Global
public record UpdateDayRequest(DateOnly Date, TimeTrackingDayType Type, IReadOnlyList<TimeTrackingEntry> Entries)
{
    public class Validator : AbstractValidator<UpdateDayRequest>
    {
    }
}