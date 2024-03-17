using System.Runtime.CompilerServices;
using domain.Common.Bases;
using domain.Common.Enums;
using domain.Common.Values;
using VEA.core.tools.OperationResult;

[assembly: InternalsVisibleTo("UnitTest")]

namespace domain.Aggregates.Events;

public class VeaEvent : Aggregate<EventId>
{
    public int MaximumNumberOfGuests { get; private set; }
    public EventStatus Status { get; private set; }
    public EventTitle Title { get; private set; }
    public EventDescription Description { get; private set; }
    public EventVisibility Visibility { get; private set; }


    public VeaEvent(EventId id) : base(id)
    {
        MaximumNumberOfGuests = 5;
        Status = EventStatus.Draft;
        Title = EventTitle.Of("Working Title").GetSuccess();
        Description = EventDescription.Of("").GetSuccess();
        Visibility = EventVisibility.Private;
    }

    public ResultVoid UpdateTitle(EventTitle title)
    {
        if (Status == EventStatus.Active)
            return ResultVoid.SingleFailure(Errors.ActiveEventCannotBeModifiedError());
        if (Status == EventStatus.Cancelled)
            return ResultVoid.SingleFailure(Errors.CancelledEventCannotBeModifiedError());
        if (title == null)
            return ResultVoid.SingleFailure(new Error(400, 400, "Title is null!"));

        Title = title;
        Status = EventStatus.Draft;

        return new ResultVoid();
    }

    public ResultVoid UpdateDescription(EventDescription description)
    {
        if (Status == EventStatus.Active)
            return ResultVoid.SingleFailure(Errors.ActiveEventCannotBeModifiedError());
        if (Status == EventStatus.Cancelled)
            return ResultVoid.SingleFailure(Errors.CancelledEventCannotBeModifiedError());

        Description = description;
        Status = EventStatus.Draft;
        return new ResultVoid();
    }

    public ResultVoid MakePublic()
    {
        if (Status == EventStatus.Cancelled)
        {
            return ResultVoid.SingleFailure(Errors.CancelledEventCannotBeModifiedError());
        }

        Visibility = EventVisibility.Public;
        return new ResultVoid();
    }

    public ResultVoid MakePrivate()
    {
        if (Status == EventStatus.Active)
            return ResultVoid.SingleFailure(Errors.ActiveEventCannotBeModifiedError());
        if (Status == EventStatus.Cancelled)
            return ResultVoid.SingleFailure(Errors.CancelledEventCannotBeModifiedError());
        if (Visibility == EventVisibility.Private)
            return new ResultVoid();
        Visibility = EventVisibility.Private;
        Status = EventStatus.Draft;
        return new ResultVoid();
    }

    public ResultVoid SetMaximumNumberOfGuests(int max)
    {
        if(Status == EventStatus.Active && max < MaximumNumberOfGuests)
            return ResultVoid.SingleFailure(Errors.MaximumNumberOfGuestsCannotBeDecreasedForActiveEvents());
        if (max < 5)
            return ResultVoid.SingleFailure(Errors.MaximumNumberOfGuestsCannotBeNegative());
        if (max > 50)
            return ResultVoid.SingleFailure(Errors.MaximumNumberOfGuestsCannotExceed50());
        if (Status == EventStatus.Cancelled)
            return ResultVoid.SingleFailure(Errors.CancelledEventCannotBeModifiedError());
        MaximumNumberOfGuests = max;
        return new ResultVoid();
    }

    public bool IsPrivate()
    {
        return Visibility switch
        {
            EventVisibility.Private => true,
            EventVisibility.Public => false,
            _ => true
        };
    }

    internal VeaEvent(EventId id, int maximumNumberOfGuests, EventStatus status, EventTitle title,
        EventDescription description, EventVisibility visibility) : base(id)
    {
        MaximumNumberOfGuests = maximumNumberOfGuests;
        Status = status;
        Title = title;
        Description = description;
        Visibility = visibility;
    }
}