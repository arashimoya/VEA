using domain.Common.Bases;
using VEA.core.tools.OperationResult;

namespace domain.Common.Values;

public class EventDescription : ValueObject
{
    public string Value { get; }

    private EventDescription(string value)
    {
        Value = value;
        Validate();
    }

    public static EventDescription Default()
    {
        return new EventDescription("");
    }
    
    public bool IsDefaultOrNull()
    {
        return string.IsNullOrEmpty(Value);
    }
    
    public static Result<EventDescription> Of(string value)
    {
        try
        {
            return new Result<EventDescription>(new EventDescription(value));
        }
        catch (Exception e)
        {

            return Result<EventDescription>.SingleFailure(new Error(400, 400, e.Message));
        }
    }

    private void Validate()
    {
        if (Value == null)
            throw new ArgumentNullException();
        if (Value.Length > 250)
            throw new ArgumentException("Description cannot be longer than 250.");
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
}