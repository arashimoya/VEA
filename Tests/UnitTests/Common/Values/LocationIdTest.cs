using domain.Common.Values;

namespace UnitTests.Common.Values;

public class LocationIdTest
{
    [Fact]
    public void should_return_a_location_id()
    {
        var result = new LocationId();
        Assert.NotNull(result);
    }
    
    [Fact]
    public void should_return_different_location_ids()
    {
        var result1 = new LocationId();
        var result2 = new LocationId();
        Assert.NotEqual(result2, result1);
    }
}