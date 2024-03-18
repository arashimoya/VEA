using domain.Common.Bases;

namespace domain.Common.Values;

public class LocationId : ValueObject
{
    public Guid Value {get; } = Guid.NewGuid();

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}