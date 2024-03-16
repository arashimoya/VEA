using domain.Common.Bases;

namespace domain.Common.Values;

public class EventTitle : ValueObject
{
    
    public string Value { get; }

    public EventTitle(string value)
    {
        Value = value;
        Validate();
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