using domain.Common.Enums;
using domain.Common.Values;

namespace UnitTests.Features.Event.SetMaximumNumberOfGuests;

public class SetMaximumNumberOfGuestsAggregateTest
{
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(50)]
    public void should_set_maximum_to_selected_value_for_draft(int max)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Draft).Build();
        
        //when
        var result = evt.SetMaximumNumberOfGuests(max);
        
        //then
        Assert.True(result.IsSuccess());
        Assert.Equal(max, evt.MaximumNumberOfGuests);
    }
    
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(50)]
    public void should_set_maximum_to_selected_value_for_ready(int max)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Ready).Build();
        
        //when
        var result = evt.SetMaximumNumberOfGuests(max);
        
        //then
        Assert.True(result.IsSuccess());
        Assert.Equal(max, evt.MaximumNumberOfGuests);
    }
    
    [Theory]
    [InlineData(5)]
    [InlineData(10)]
    [InlineData(25)]
    [InlineData(50)]
    public void should_set_maximum_to_selected_value_for_active(int max)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithMaximumNumberOfGuests(5).WithStatus(EventStatus.Active).Build();
        
        //when
        var result = evt.SetMaximumNumberOfGuests(max);
        
        //then
        Assert.True(result.IsSuccess());
        Assert.Equal(max, evt.MaximumNumberOfGuests);
    }

    [Fact]
    public void should_throw_when_reducing_max_for_active_event()
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithMaximumNumberOfGuests(50).WithStatus(EventStatus.Active).Build();
        
        //when
        var result = evt.SetMaximumNumberOfGuests(49);
        
        //Then
        Assert.False(result.IsSuccess());
    }
    
    [Fact]
    public void should_throw_for_cancelled_event()
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Cancelled).Build();
        
        //when
        var result = evt.SetMaximumNumberOfGuests(49);
        
        //Then
        Assert.False(result.IsSuccess());
    }
    
    [Fact]
    public void should_throw_too_small_max()
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).Build();
        
        //when
        var result = evt.SetMaximumNumberOfGuests(4);
        
        //Then
        Assert.False(result.IsSuccess());
    }
    
    [Fact]
    public void should_throw_for_max_larger_than_locations_max()
    {
        //need location implementation
        Assert.False(true);
    }
    
    [Fact]
    public void should_throw_for_max_larger_than_50()
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).Build();
        //when
        var result = evt.SetMaximumNumberOfGuests(51);
        //then
        Assert.False(result.IsSuccess());
    }
}