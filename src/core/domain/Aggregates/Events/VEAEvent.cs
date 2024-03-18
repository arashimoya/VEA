using System.Runtime.CompilerServices;
using domain.Aggregates.Users;
using domain.Common.Bases;
using domain.Common.Contracts;
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
    public EventInterval Interval { get; private set; }
    
    public List<User> Guests { get; private set; }

    public static VeaEvent Create()
    {
        return new VeaEvent(new EventId());
    }


    public VeaEvent(EventId id) : base(id)
    {
        MaximumNumberOfGuests = 5;
        Status = EventStatus.Draft;
        Title = EventTitle.Default();
        Description = EventDescription.Default();
        Visibility = EventVisibility.Private;
        Guests = new List<User>();
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
        if (Status == EventStatus.Active && max < MaximumNumberOfGuests)
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

    public ResultVoid MakeReady(ICurrentTimeProvider currentTimeProvider)
    {
        var errors = CheckErrorsForMakingReady(currentTimeProvider).ToList();
        if (errors.Count > 0)
            return new ResultVoid(errors);
        Status = EventStatus.Ready;
        return new ResultVoid();
    }
    
    public ResultVoid Activate(ICurrentTimeProvider currentTimeProvider)
    {
        var result = MakeReady(currentTimeProvider);
        if (!result.IsSuccess()) return result;
        
        Status = EventStatus.Active;
        return new ResultVoid();

    }

    private IEnumerable<Error> CheckErrorsForMakingReady(ICurrentTimeProvider currentTimeProvider)
    {
        if (Title == null || Title.IsDefaultOrNullOrEmpty())
            yield return Errors.TitleNotSetOrDefault();
        if (Description == null || Description.IsDefaultOrNull())
            yield return Errors.DescriptionNotSetOrDefault();
        if (Interval != null && Interval.Start.CompareTo(currentTimeProvider.now()) < 0)
            yield return Errors.StartPriorToTimeOfReadying();
        if (Interval == null)
            yield return Errors.TimesNotSet();
        if (MaximumNumberOfGuests is < 5 or > 50)
            yield return Errors.InvalidMaxNumberOfGuests();
        if (Status == EventStatus.Cancelled)
            yield return Errors.CancelledEventCannotBeModifiedError();
    }

    public ResultVoid UpdateTimes(EventInterval interval, ICurrentTimeProvider currentTimeProvider)
    {
        if (Status == EventStatus.Active)
            return ResultVoid.SingleFailure(Errors.ActiveEventCannotBeModifiedError());
        if (Status == EventStatus.Cancelled)
            return ResultVoid.SingleFailure(Errors.CancelledEventCannotBeModifiedError());
        if (interval.Start.CompareTo(currentTimeProvider.now()) < 0)
            return ResultVoid.SingleFailure(Errors.StartInThePast());
        Interval = interval;
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

    public ResultVoid RegisterGuestTo(User user, ICurrentTimeProvider currentTimeProvider)
    {
        if (!Status.Equals(EventStatus.Active))
        {
            return ResultVoid.SingleFailure(Errors.EventNotActive());
        }
        if (MaximumNumberOfGuests == Guests.Count)
        {
            return ResultVoid.SingleFailure(Errors.CapacityReached());
        }

        if (Interval.Start.CompareTo(currentTimeProvider.now()) < 0)
        {
            return ResultVoid.SingleFailure(Errors.OngoingEvent());
        }

        if (IsPrivate())
        {
            return ResultVoid.SingleFailure(Errors.PrivateEvent());
        }

        if (Guests.Contains(user))
        {
            return ResultVoid.SingleFailure(Errors.AlreadyParticipating());
        }
        Guests.Add(user);
        return new ResultVoid();
    }

    internal VeaEvent(EventId id, int maximumNumberOfGuests, EventStatus status, EventTitle title,
        EventDescription description, EventVisibility visibility, EventInterval interval, List<User> guests) : base(id)
    {
        MaximumNumberOfGuests = maximumNumberOfGuests;
        Status = status;
        Title = title;
        Description = description;
        Visibility = visibility;
        Interval = interval;
        Guests = guests ?? [];
    }
}