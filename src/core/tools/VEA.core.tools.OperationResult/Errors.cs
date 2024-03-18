namespace VEA.core.tools.OperationResult;

public class Errors
{
    public static Error ActiveEventCannotBeModifiedError()
    {
        return new Error(405, 10000, "Active event cannot be modified");
    }

    public static Error CancelledEventCannotBeModifiedError()
    {
        return new Error(405, 10001, "Cancelled event cannot be modified");
    }
    
    public static Error MaximumNumberOfGuestsCannotBeNegative()
    {
        return new Error(400, 10002, "Maximum number of guests cannot be negative");
    }
    
    public static Error MaximumNumberOfGuestsCannotBeDecreasedForActiveEvents()
    {
        return new Error(400, 10003, "Maximum number of guests cannot be decreased for active events");
    }
    
    public static Error MaximumNumberOfGuestsCannotExceed50()
    {
        return new Error(400, 10004, "Maximum number of guests cannot exceed 50");
    }
    
    public static Error TitleNotSetOrDefault()
    {
        return new Error(400, 10005, "Title is not set or is a default value");
    }
    
    public static Error DescriptionNotSetOrDefault()
    {
        return new Error(400, 10007, "Description is not set or is a default value");
    }
    
    
    public static Error StartTimeAfterEndTime()
    {
        return new Error(400, 10009, "Start time cannot be after end time");
    }
    
    public static Error TooShortDuration()
    {
        return new Error(400, 10010, "Duration cannot be shorter than 60 minutes.");
    }
    
    public static Error StartBefore8AM()
    {
        return new Error(400, 10011, "Start cannot be before 8AM.");
    }
    
    public static Error EndAfter1AM()
    {
        return new Error(400, 10012, "End cannot be after 1AM.");
    }
    
    public static Error StartInThePast()
    {
        return new Error(400, 10013, "Start cannot be in the past unless you're a time traveller");
    }
    public static Error DurationTooLong()
    {
        return new Error(400, 10014, "Duration cannot be longer than 10h");
    }

    public static Error TimesNotSet()
    {
        return new Error(400, 10015, "Times are not set");
    }
    
    public static Error InvalidMaxNumberOfGuests()
    {
        return new Error(400, 10017, "Max number of guests should be between 5 and 50");
    }
    
    public static Error StartPriorToTimeOfReadying()
    {
        return new Error(400, 10018, "Start date cannot be prior to readying the event.");
    }

    public static Error DoesNotMatchEmailPattern(string value)
    {
        return new Error(400, 10019, "This (" + value + ") is not a valid email");
    }
    public static Error IsEmpty()
    {
        return new Error(400, 10020, "Given value is empty");
    }
}