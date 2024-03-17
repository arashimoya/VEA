using domain.Common.Enums;
using domain.Common.Values;

namespace UnitTests.Features.Event.MakePublic;

public class MakePublicAggregateTest
{
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    [InlineData(EventStatus.Active)]
    public void should_make_event_public(EventStatus status)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(status).WithIsPrivate(EventVisibility.Private).Build();
        
        //when
        var result = evt.MakePublic();
        
        //Then
        Assert.True(result.IsSuccess());
        Assert.False(evt.IsPrivate());
        Assert.Equal(status, evt.Status);
    }
    
    [Fact]
    public void should_throw_if_cancelled()
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Cancelled).Build();
        
        //when
        var result = evt.MakePublic();
        
        //Then
        Assert.False(result.IsSuccess());
    }
}