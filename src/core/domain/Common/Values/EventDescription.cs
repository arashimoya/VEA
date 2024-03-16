using domain.Common.Bases;

namespace domain.Common.Values;

public class EventDescription : ValueObject
{
    public string Value { get; }

    public EventDescription(string value)
    {
        Value = value;
        Validate();
    }

    private void Validate()
    {
        if (Value == null)
            throw new ArgumentNullException();
        if (Value.Length > 1024)
            throw new ArgumentException("Description cannot be longer than 1024.");
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
    
}