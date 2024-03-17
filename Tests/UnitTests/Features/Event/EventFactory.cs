using domain.Aggregates.Events;
using domain.Common.Enums;
using domain.Common.Values;
using UnitTests.Stubs;

namespace UnitTests.Features.Event;

public class EventFactory
{
    private EventId _id;
    private int _maximumNumberOfGuests;
    private EventStatus _status;
    private EventTitle _title;
    private EventDescription _description;
    private EventVisibility _isPrivate;
    private EventInterval _interval;

    public EventFactory WithId(EventId id)
    {
        _id = id;
        return this;
    }
    public EventFactory WithMaximumNumberOfGuests(int maximumNumberOfGuests)
    {
        _maximumNumberOfGuests = maximumNumberOfGuests;
        return this;
    }
    public EventFactory WithStatus(EventStatus eventStatus)
    {
        _status = eventStatus;
        return this;
    }
    
    public EventFactory WithTitle(EventTitle title)
    {
        _title = title;
        return this;
    }
    
    public EventFactory WithDescription(EventDescription description)
    {
        _description = description;
        return this;
    }
    
    public EventFactory WithIsPrivate(EventVisibility isPrivate)
    {
        _isPrivate = isPrivate;
        return this;
    }
    
    public EventFactory WithTimes(EventInterval interval)
    {
        _interval = interval;
        return this;
    }

    public VeaEvent Build()
    {
        return new VeaEvent(_id, _maximumNumberOfGuests, _status, _title, _description, _isPrivate, _interval);
    }
    
    public static EventDescription TestDescription()
    {
        return EventDescription.Of("description").GetSuccess();
    }

    public static EventInterval TestInterval()
    {
        var stub = new StubCurrentTimeProvider();
        return EventInterval.of(stub.now(), stub.now().AddHours(3)).GetSuccess();
    }

    public static EventTitle TestTitle()
    {
        return EventTitle.Of("title").GetSuccess();
    }
}