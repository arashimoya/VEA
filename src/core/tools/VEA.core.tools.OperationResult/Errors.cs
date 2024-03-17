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
    
    public static Error TitleNotSet()
    {
        return new Error(400, 10005, "Title is not set");
    }
    
    public static Error DefaultTitle()
    {
        return new Error(400, 10006, "Cannot ready an event with a default title");
    }
    
    public static Error DescriptionNotSet()
    {
        return new Error(400, 10005, "Description is not set");
    }
    
    public static Error DefaultDescription()
    {
        return new Error(400, 10006, "Cannot ready an event with a default description");
    }
}