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
}