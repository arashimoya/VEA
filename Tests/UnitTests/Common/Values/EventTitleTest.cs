using domain.Common.Values;

namespace UnitTests.Common.Values;

public class EventTitleTest
{
    [Fact]
    public void should_create_an_event_title()
    {
        //given
        var str = "a great title";
        
        var result = EventTitle.Of(str);
        Assert.True(result.IsSuccess());
        Assert.NotNull(result.GetSuccess());
        Assert.NotNull(result.GetSuccess().Value);
        Assert.NotEqual(0, result.GetSuccess().Value.Length);
    }
    
    [Fact]
    public void should_throw_when_invalid_constructor_argument_null()
    {
        var result = EventTitle.Of(null!);
        Assert.False(result.IsSuccess());
    }
    
    [Fact]
    public void should_throw_when_invalid_constructor_argument_empty()
    {
        var emptyStr = "";
        var result = EventTitle.Of(emptyStr);
        Assert.False(result.IsSuccess());
        Assert.Equal("Title cannot be empty.",result.Errors.First().DeveloperMessage);
    }
    
    [Fact]
    public void should_throw_when_invalid_constructor_argument_too_long()
    {
        var tooLongString = "DU7LYjw8rDt5iUhAXniRaRPKKD59n1VxuAKvBVHP4fnAmi1jjATyBiT5nDPEnEBr8LtbVJbrTkT4B2PfqAtHCEJ4XFj6A0DevNpxrpLCcdwdwwJp7XZ3vpieDBDjzYLfWqUwd2HXGj9mDkFbLtGEBRP3QUcY1dxQjv4S7KRd0hC8UX9rMMM2yuXL9wwR5Lef79D5gCftVp8hWW6v3a8Qwuv1LwazEAK5zXWYhKk3BkDf5uxERVubbCHBxyU8ddXyZLYM8LHqgZ60Lg2AnUJ2a370";
        var result = EventTitle.Of(tooLongString);
        Assert.False(result.IsSuccess());
        Assert.Equal("Title cannot be longer than 256.",result.Errors.First().DeveloperMessage);
    }
    
}