using domain.Common.Values;

namespace UnitTests.Common.Values;

public class IntervalIdTest
{
    [Fact]
    public void should_return_a_interval_id()
    {
        var result = new IntervalId();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void should_return_different_interval_ids()
    {
        var result1 = new IntervalId();
        var result2 = new IntervalId();
        Assert.NotEqual(result2, result1);
    }
}