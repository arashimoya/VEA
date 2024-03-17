using domain.Common.Contracts;

namespace UnitTests.Stubs;

public class StubCurrentTimeProvider : ICurrentTimeProvider
{
    public DateTime now()
    {
        return new DateTime(2023, 5, 10, 10,10,10);
    }
}