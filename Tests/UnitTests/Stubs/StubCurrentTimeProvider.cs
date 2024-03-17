using domain.Common.Contracts;

namespace UnitTests.Stubs;

public class StubCurrentTimeProvider : ICurrentTimeProvider
{
    
    public DateTime DateTime { get; set; } = new DateTime(2023, 5, 10, 10,10,10);
    public DateTime now()
    {
        return DateTime;
    }
}