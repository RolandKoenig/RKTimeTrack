using FluentValidation;
using RolandK.TimeTrack.Application.Models;

namespace RolandK.TimeTrack.Application.UseCases;

public record UpdateDay_Request(DateOnly Date, TimeTrackingDayType Type, IReadOnlyList<TimeTrackingEntry> Entries)
{
    public class Validator : AbstractValidator<UpdateDay_Request>
    {
    }
}