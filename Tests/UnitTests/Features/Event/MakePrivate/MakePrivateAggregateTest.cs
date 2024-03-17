using domain.Common.Enums;
using domain.Common.Values;

namespace UnitTests.Features.Event.MakePrivate;

public class MakePrivateAggregateTest
{
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    public void should_remain_private(EventStatus status)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(status).WithIsPrivate(EventVisibility.Private).Build();
        
        //when
        var result = evt.MakePrivate();
        
        //Then
        Assert.True(result.IsSuccess());
        Assert.True(evt.IsPrivate());
        Assert.Equal(status, evt.Status);
    }
    
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    public void should_make_private(EventStatus status)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(status).WithIsPrivate(EventVisibility.Public).Build();
        
        //when
        var result = evt.MakePrivate();
        
        //Then
        Assert.True(result.IsSuccess());
        Assert.True(evt.IsPrivate());
        Assert.Equal(EventStatus.Draft, evt.Status);
    }
    
    [Fact]
    public void should_throw_if_active()
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Active).WithIsPrivate(EventVisibility.Private).Build();
        
        //when
        var result = evt.MakePrivate();
        
        //Then
        Assert.False(result.IsSuccess());
    }
    
    [Fact]
    public void should_throw_if_cancelled()
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Cancelled).WithIsPrivate(EventVisibility.Private).Build();
        
        //when
        var result = evt.MakePrivate();
        
        //Then
        Assert.False(result.IsSuccess());
    }
}