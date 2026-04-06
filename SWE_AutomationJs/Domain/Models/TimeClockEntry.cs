namespace Server.Data.Entities;

public sealed class TimeClockEntry
{
    public int TimeClockEntryId { get; set; }

    public int EmployeeId { get; set; }
    public Employee Employee { get; set; } = default!;

    public DateTime ClockInAt { get; set; }
    public DateTime? ClockOutAt { get; set; }

    // Computed for convenience (can also compute in query)
    public double? HoursWorked { get; set; }
}
