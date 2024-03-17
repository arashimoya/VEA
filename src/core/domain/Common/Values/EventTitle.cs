using domain.Common.Bases;
using VEA.core.tools.OperationResult;

namespace domain.Common.Values;

public class EventTitle : ValueObject
{
    
    public string Value { get; }

    private EventTitle(string value)
    {
        Value = value;
        Validate();
    }

    public static Result<EventTitle> Of(string value)
    {
        try
        {
            return new Result<EventTitle>(new EventTitle(value));
        }
        catch (Exception e)
        {

            return Result<EventTitle>.SingleFailure(new Error(400, 400, e.Message));
        }
    }

    private void Validate()
    {
        if (Value == null)
            throw new ArgumentNullException();
        if (Value.Length > 256)
            throw new ArgumentException("Title cannot be longer than 256.");
        if (Value.Length == 0)
            throw new ArgumentException("Title cannot be empty.");
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}