using domain.Common.Values;

namespace UnitTests.Common.Values;

public class EventIdTest
{
    [Fact]
    public void should_return_a_event_id()
    {
        var result = new EventId();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void should_return_different_event_ids()
    {
        var result1 = new EventId();
        var result2 = new EventId();
        Assert.NotEqual(result2, result1);
    }
}