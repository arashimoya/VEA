using System.Runtime.CompilerServices;
using domain.Common.Bases;
using domain.Common.Enums;
using domain.Common.Values;
using VEA.core.tools.OperationResult;

[assembly: InternalsVisibleTo("UnitTest")]

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
        Title = EventTitle.Of("Working Title").GetSuccess();
        Description = new EventDescription("");
        IsPrivate = true;
    }

    public ResultVoid UpdateTitle(EventTitle title)
    {
        throw new NotImplementedException();
    }

    internal VeaEvent(EventId id, int maximumNumberOfGuests, EventStatus status, EventTitle title, EventDescription description, bool isPrivate) : base(id)
    {
        MaximumNumberOfGuests = maximumNumberOfGuests;
        Status = status;
        Title = title;
        Description = description;
        IsPrivate = isPrivate;
    }
}