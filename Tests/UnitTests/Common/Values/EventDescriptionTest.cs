using domain.Common.Values;

namespace UnitTests.Common.Values;

public class EventDescriptionTest
{
    [Fact]
    public void should_create_an_event_description()
    {
        //given
        var str = "a great title";
        
        var result = EventDescription.Of(str);
        Assert.True(result.IsSuccess());
        Assert.NotNull(result.GetSuccess().Value);
        Assert.NotEqual(0, result.GetSuccess().Value.Length);
    }
    
    [Fact]
    public void should_throw_when_invalid_constructor_argument_null()
    {
        var result = EventDescription.Of(null!);
        Assert.False(result.IsSuccess());
    }
    
    [Fact]
    public void should_create_an_empty_description()
    {
        var str = "";
        
        var result = EventDescription.Of(str);
        Assert.True(result.IsSuccess());
        Assert.NotNull(result.GetSuccess().Value);
        Assert.Equal(0, result.GetSuccess().Value.Length);
    }
    
    [Fact]
    public void should_throw_when_invalid_constructor_argument_too_long()
    {
        var tooLongString = "0n8TzGKWkBZSC7NdMM9e2u7Lq5dp88GNpVk7AV3AvGATNzQPcRaKJFw5LKJSGqb8PxaCEnWMmq3qcR0aWjyeLL6J7rnrUpD2GMXUw2PfrVThNyrEu1Uyj9FJt8jfpWBw4vd6e7hAA6aAxbckxYjVJfmHfS1gHHZqgrZ7vzdph5DhVAzLtScCy3UYzC6XNYK0FVaecujC5w8vCBSMkGechagvCrGfEC9baM1F0CLDpgf3v0crj8EeVD12fvaxH5fr1V6hV4NZjBtD9kcqgcWUrfp9XJzkzjPgQe3S1mCj60zZRjVmgeJTXmprpuVYzNydzRjCuUd43mEYc1A8ZKZ8fVEhVxTTTaQjjnUdPtUAMW4fGMdrxkdEP5Sw9Cy1eE5Qcjk27k2KB3pv8NLhHm2FPb0td0fhXgzJbC1x8YXJLSMKUVtAaKnB3YAMvHPWcP8mgjmZSm6giJQTwkKFn2Ffj5NydYQpZkEW4gLrSJ0KFYdtkACXZBRtmQiNjKjLJjrrg6R8ThwLFu2Vva5SSceX6kaTHCavT9yhGgtGr4hQJBPj1rtnJPbijar05dCDU4ZPA1Mgz8LMtnACdDXz7nhJAJweZM6dQ4BWX1jn4Z6cKrjWTV1KG7zivqQNDKqcvwWGeCP4kXF0EdKy0qkUYTXWU0a3meVu6dfJWgqv8i4LjWb1pkKrkGB8RDEGuAxVSD680XDt8wQU9RDL63iWzHzkxUqYMC5G8mZfpQKQnWeQzG3z8MajxbqEF5b5XjSViwUEBWYpU2WfJiBAWUCfFDJDhV6AhY9gN5dQzMveniN3zYxEhYgw9EbA0mkBYCW2PnEhvhcJ8QJeknm5AUThi9tWYumdcDRmpcEUNNj2k6dJnhdJ2bVDEkYVVGANVkhDQ05cYzvHRHZ4UxmMqeQFadB5f40kz5AwfTB2Nip0YGV20d9tFbrD2RefkpHMedYDCrzyazPHZMGnV8At40190zpft1dtmJr96t7heTHcEx9mcBq1eRDFCjMBP4gd41tiu6Ht6qnvvnMV9ifx1nK4UPZD1irPSb";
        var result = EventDescription.Of(tooLongString);
        Assert.False(result.IsSuccess());
        Assert.Equal("Description cannot be longer than 250.",result.Errors.First().DeveloperMessage);
    }
    
    
}