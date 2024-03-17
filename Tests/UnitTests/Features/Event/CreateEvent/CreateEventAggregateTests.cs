using domain.Aggregates.Events;
using domain.Common.Enums;
using domain.Common.Values;

namespace UnitTests.Features.Event.CreateEvent;

public class CreateEventAggregateTests
{
    [Fact]
    public void should_create_an_event_with_max_guests_of_5_and_status_draft()
    {
        //Given 
        var id = new EventId();

        //When
        var result = new VeaEvent(id);

        //Then
        Assert.Equal(5, result.MaximumNumberOfGuests);
        Assert.Equal(EventStatus.Draft, result.Status);
        Assert.NotNull(result.Id);
    }

    [Fact]
    public void should_create_an_event_with_default_title()
    {
        //Given 
        var id = new EventId();

        //When
        var result = new VeaEvent(id);

        //Then
        Assert.Equal("Working Title", result.Title.Value);
        Assert.NotNull(result.Id);
    }
    
    [Fact]
    public void should_create_an_event_with_default_description()
    {
        //Given 
        var id = new EventId();

        //When
        var result = new VeaEvent(id);

        //Then
        Assert.Equal("", result.Description.Value);
        Assert.NotNull(result.Id);
    }
    
    [Fact]
    public void should_create_an_event_with_default_private_visibility()
    {
        //Given 
        var id = new EventId();

        //When
        var result = new VeaEvent(id);

        //Then
        Assert.Equal(EventVisibility.Private,result.Visibility);
        Assert.NotNull(result.Id);
    }
    
}