using System.Collections.Immutable;
using RolandK.TimeTrack.Application.Models;
using RolandK.TimeTrack.Application.Util;
using RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Migrations;

namespace RolandK.TimeTrack.FileBasedTimeTrackingRepositoryAdapter.Data;

class TimeTrackingStore
{
    private readonly TimeProvider _timeProvider;
    
    private DateTimeOffset _lastChangeTimestamp = DateTimeOffset.MinValue;
    private ImmutableList<TimeTrackingDay> _store = ImmutableList<TimeTrackingDay>.Empty;

    public ImmutableList<TimeTrackingDay> Store => _store;
    
    public DateTimeOffset LastChangeTimestamp => _lastChangeTimestamp;

    public TimeTrackingStore(TimeProvider timeProvider)
    {
        _timeProvider = timeProvider;
    }
    
    public void Reset()
    {
        this.SetCurrentStore(
            ImmutableList<TimeTrackingDay>.Empty,
            DateTimeOffset.MinValue);
    }
    
    public TimeTrackingDocument StoreToDocument()
    {
        return new TimeTrackingDocument(
            TimeTrackingDocumentMigrator.CURRENT_VERSION.ToString(),
            this.Store);
    }
    
    public void RestoreFromDocument(TimeTrackingDocument document)
    {
        this.SetCurrentStore(
            document.Days.ToImmutableList(),
            DateTimeOffset.MinValue);
    }

    public IReadOnlyList<TimeTrackingDay> GetAllDaysInAscendingOrderAsync() => this.Store;
    
    /// <summary>
    /// Is there a <see cref="TimeTrackingDay"/> for the given Date?
    /// </summary>
    public bool ContainsDay(DateOnly date) 
        => SearchForDayIndex(date) > -1;
    
    /// <summary>
    /// Gets a <see cref="TimeTrackingDay"/> for the given date.
    /// A new <see cref="TimeTrackingDay"/> is created, if there was no one found.
    /// </summary>
    public TimeTrackingDay GetOrCreateDay(DateOnly date)
    {
        var existingDay = SearchForDay(date);
        if (existingDay != null)
        {
            return existingDay;
        }
        
        return CreateAndInsertNewDayWithDefaults(date);
    }

    /// <summary>
    /// Gets a <see cref="TimeTrackingWeek"/> for the given year and week number.
    /// A new <see cref="TimeTrackingDay"/> is created for each date, where no <see cref="TimeTrackingDay"/> was found.
    /// </summary>
    public TimeTrackingWeek GetOrCreateWeek(int year, int weekNumber)
    {
        var dateOfMonday = GermanCalendarWeekUtil.GetDateOfMonday(year, weekNumber);
        return new TimeTrackingWeek(
            year, 
            weekNumber,
            monday: GetOrCreateDay(dateOfMonday),
            tuesday: GetOrCreateDay(dateOfMonday.AddDays(1)),
            wednesday: GetOrCreateDay(dateOfMonday.AddDays(2)),
            thursday: GetOrCreateDay(dateOfMonday.AddDays(3)),
            friday: GetOrCreateDay(dateOfMonday.AddDays(4)),
            saturday: GetOrCreateDay(dateOfMonday.AddDays(5)),
            sunday: GetOrCreateDay(dateOfMonday.AddDays(6)));
    }

    /// <summary>
    /// Adds a new <see cref="TimeTrackingDay"/> or updates an existing one.
    /// </summary>
    public TimeTrackingDay AddOrUpdateDay(TimeTrackingDay day)
    {
        var existingRowIndex = SearchForDayIndex(day.Date);
        if (existingRowIndex > -1)
        {
            this.SetCurrentStore(this.Store.SetItem(existingRowIndex, day));
            _lastChangeTimestamp = _timeProvider.GetUtcNow();
            return day;
        }

        this.SetCurrentStore(this.Store.Insert(~existingRowIndex, day));
        _lastChangeTimestamp = _timeProvider.GetUtcNow();
        return day;
    }

    private TimeTrackingDay CreateAndInsertNewDayWithDefaults(DateOnly date)
    {
        var existingRowIndex = SearchForDayIndex(date);
        if (existingRowIndex > -1)
        {
            throw new ArgumentException($"There is already a {nameof(TimeTrackingDay)} for date {date}");
        }
        
        var dayType = date.DayOfWeek switch
        {
            DayOfWeek.Saturday => TimeTrackingDayType.Weekend,
            DayOfWeek.Sunday => TimeTrackingDayType.Weekend,
            _ => TimeTrackingDayType.WorkingDay
        };

        var newDay = new TimeTrackingDay(
            date: date,
            type: dayType,
            entries: Array.Empty<TimeTrackingEntry>());
        this.SetCurrentStore(this.Store.Insert(~existingRowIndex, newDay));
        _lastChangeTimestamp = _timeProvider.GetUtcNow();
        return newDay;
    }
    
    /// <summary>
    /// Searches for the day with the given date.
    /// Returns null, if there is no data found for the given date.
    /// </summary>
    private TimeTrackingDay? SearchForDay(DateOnly date)
    {
        var index = SearchForDayIndex(date);
        if (index > -1)
        {
            return this.Store[index];
        }

        return null;
    }
    
    /// <summary>
    /// Searches for the day with the given date.
    /// Returns -1, if there is no data found for the given date.
    /// </summary>
    private int SearchForDayIndex(DateOnly date)
    {
        // Binary search algorithm by
        // https://tutorials.eu/binary-search-in-c-sharp/
        
        var left = 0;
        var right = this.Store.Count - 1;
        while (left <= right)
        {
            var middle = (left + right) / 2;
            var comparison = this.Store[middle].Date.CompareTo(date);
            if (comparison == 0)
            {
                return middle;
            }
            else if (comparison < 0)
            {
                left = middle + 1;
            }
            else
            {
                right = middle - 1;
            }
        }
        return ~left;
    }
    
    private void SetCurrentStore(ImmutableList<TimeTrackingDay> store)
    {
        this.SetCurrentStore(store, _timeProvider.GetLocalNow());
    }
    
    private void SetCurrentStore(
        ImmutableList<TimeTrackingDay> store,
        DateTimeOffset lastChangeTimestamp)
    {
        _store = store;
        _lastChangeTimestamp = lastChangeTimestamp;
    }
}