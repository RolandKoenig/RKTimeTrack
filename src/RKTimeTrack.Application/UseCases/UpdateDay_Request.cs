using FluentValidation;
using RKTimeTrack.Application.Models;

namespace RKTimeTrack.Application.UseCases;

public record UpdateDay_Request(DateOnly Date, TimeTrackingDayType Type, IReadOnlyList<TimeTrackingEntry> Entries)
{
    public class Validator : AbstractValidator<UpdateDay_Request>
    {
    }
}