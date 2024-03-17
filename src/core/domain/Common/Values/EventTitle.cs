using domain.Common.Bases;
using VEA.core.tools.OperationResult;

namespace domain.Common.Values;

public class EventTitle : ValueObject
{
    
    public string Value { get; }
    private static string DefaultValue => "Working Title";

    private EventTitle(string value)
    {
        Value = value;
        Validate();
    }

    public static EventTitle Default()
    {
        return new EventTitle(DefaultValue);
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

    public bool IsDefaultOrNullOrEmpty()
    {
        return string.IsNullOrEmpty(Value) || string.IsNullOrWhiteSpace(Value) || Value.Equals(DefaultValue);
    }

    private void Validate()
    {
        if (Value == null)
            throw new ArgumentNullException(null,"Title is null!");
        if (Value.Length > 75)
            throw new ArgumentException("Title too long! Length should be between 3 and 75");
        if (Value.Length == 0)
            throw new ArgumentException("Title cannot be empty.");
        if (Value.Length < 3)
            throw new ArgumentException("Title too short! Length should be between 3 and 75");
        
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}