using domain.Aggregates.Events;
using domain.Common.Enums;
using domain.Common.Values;

namespace UnitTests.Features.Event.UpdateTitle;

public class UpdateTitleAggregateTest
{
    [Fact]
    public void should_update_title_s1()
    {
        //Given an event
        var veaEvent = new VeaEvent(new EventId());
        var titleStr = "VIA Hackathon";

        //When
        var result = veaEvent.UpdateTitle(EventTitle.Of(titleStr).GetSuccess());

        //Then
        Assert.Equal(titleStr, veaEvent.Title.Value);
        Assert.True(result.IsSuccess());
    }

    [Fact]
    public void should_update_title_s2()
    {
        //Given an event
        var veaEvent = new EventFactory()
            .WithId(new EventId())
            .WithStatus(EventStatus.Ready)
            .Build();
        var titleStr = "VIA Hackathon";


        //When
        var result = veaEvent.UpdateTitle(EventTitle.Of(titleStr).GetSuccess());

        //Then
        Assert.True(result.IsSuccess());
        Assert.Equal(titleStr, veaEvent.Title.Value);
        Assert.Equal(EventStatus.Draft, veaEvent.Status);
    }
    
    [Fact]
    public void should_throw_when_title_length_0()
    {
        //Given an event
        var titleStr = "";
        
        //When
        var result = EventTitle.Of(titleStr);

        //Then
        Assert.False(result.IsSuccess());
        Assert.Equal("Title cannot be empty.", result.Errors.First().DeveloperMessage);
        
    }
    
    [Theory]
    [InlineData("XY")]
    [InlineData("a")]
    public void should_throw_when_title_length_too_short(string titleStr)
    {
        //Given an event
        
        //When
        var result = EventTitle.Of(titleStr);

        //Then
        Assert.False(result.IsSuccess());
        Assert.Equal("Title too short! Length should be between 3 and 75", result.Errors.First().DeveloperMessage);
        
    }
    
    [Fact]
    public void should_throw_when_title_length_too_long()
    {
        //Given a 80-char string
        var titleStr = "12JrzyTVHJvyg1KtL9eDYDGuYUCeXMrJwxm4BwF6Jr2CXEZFRcwY3f5eEz2LdWidLxdH8dqT2mU4TAL1";
        
        //When
        var result = EventTitle.Of(titleStr);

        //Then
        Assert.False(result.IsSuccess());
        Assert.Equal("Title too long! Length should be between 3 and 75", result.Errors.First().DeveloperMessage);
        
    }
    
    [Fact]
    public void should_throw_when_title_null_string_case()
    {
        //Given an event
        
        //When
        var result = EventTitle.Of(null!);

        //Then
        Assert.False(result.IsSuccess());
        Assert.Equal("Title is null!", result.Errors.First().DeveloperMessage);
        
    }
    
    [Fact]
    public void should_throw_when_title_null_object_case()
    {
        //Given an event
        var evt = new EventFactory().WithId(new EventId()).Build();
        
        //When
        var result = evt.UpdateTitle(null!);

        //Then
        Assert.False(result.IsSuccess());
        Assert.Equal("Title is null!", result.Errors.First().DeveloperMessage);
        
    }
    
    [Fact]
    public void should_throw_when_event_is_active()
    {
        //Given an event
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Active).Build();
        
        //When
        var result = evt.UpdateTitle(EventTitle.Of("title").GetSuccess());

        //Then
        Assert.False(result.IsSuccess());
        Assert.Equal("Active event cannot be modified", result.Errors.First().DeveloperMessage);
        
    }
    
    [Fact]
    public void should_throw_when_event_is_cancelled()
    {
        //Given an event
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Cancelled).Build();
        
        //When
        var result = evt.UpdateTitle(EventTitle.Of("title").GetSuccess());

        //Then
        Assert.False(result.IsSuccess());
        Assert.Equal("Cancelled event cannot be modified", result.Errors.First().DeveloperMessage);
        
    }
}