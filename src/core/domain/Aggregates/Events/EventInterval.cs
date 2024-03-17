using System.ComponentModel.DataAnnotations;
using domain.Common.Bases;
using domain.Common.Values;
using VEA.core.tools.OperationResult;

namespace domain.Aggregates.Events;

public class EventInterval : Entity<IntervalId>
{
    public DateTime Start { get; private set; }
    public DateTime End { get; private set; }

    public static Result<EventInterval> of(DateTime start, DateTime end)
    {
        var possibleErrors = CheckForErrors(start, end).ToList();
        if (possibleErrors.Count > 0)
            return new Result<EventInterval>(possibleErrors);

        return new Result<EventInterval>(new EventInterval
        {
            Start = start,
            End = end
        });
    }

    private static IEnumerable<Error> CheckForErrors(DateTime start, DateTime end)
    {
        if (!IsBefore(start, end))
            yield return Errors.StartTimeAfterEndTime();
        if (DurationInHours(start, end) <= 1)
            yield return Errors.TooShortDuration();
        if (DurationInHours(start, end) > 10)
            yield return Errors.DurationTooLong();
        if(!ValidStart(start))
            yield return Errors.StartBefore8AM();
        if(!ValidEnd(end))
            yield return Errors.EndAfter1AM();
    }

    private static bool IsBefore(DateTime start, DateTime end)
    {
        return start.CompareTo(end) < 0;
    }

    private static double DurationInHours(DateTime start, DateTime end)
    {
        return (end - start).TotalHours;
    }
    
    private static bool ValidStart(DateTime start)
    {
        return start.Hour >= 8;
    }
    
    private static bool ValidEnd(DateTime end)
    {
        return (end.Hour >= 9 && end.Hour <= 23) || (end.Hour >= 0 && (end.Hour <= 1 && end.Minute == 0));
    }
}