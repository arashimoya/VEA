using domain.Common.Bases;
using domain.Common.Enums;
using domain.Common.Values;

namespace domain.Aggregates.Events;

public class VeaEvent : Aggregate<EventId>
{
    public int MaximumNumberOfGuests { get; }
    public EventStatus Status { get; }
    public EventTitle Title { get; }
    public EventDescription Description { get; }
    public bool IsPrivate { get; }


    public VeaEvent(EventId id) : base(id)
    {
        MaximumNumberOfGuests = 5;
        Status = EventStatus.Draft;
        Title = new EventTitle("Working Title");
        Description = new EventDescription("");
        IsPrivate = true;
    }
}