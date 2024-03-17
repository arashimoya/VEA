using domain.Common.Enums;
using domain.Common.Values;
using UnitTests.Stubs;
using VEA.core.tools.OperationResult;

namespace UnitTests.Features.Event.ActivateEvent;

public class ActivateEventAggregateTest
{
    [Fact]
    public void should_activate_draft_event()
    {
        var evt = new EventFactory().WithId(new EventId())
            .WithStatus(EventStatus.Draft)
            .WithDescription(EventFactory.TestDescription())
            .WithTitle(EventFactory.TestTitle())
            .WithIsPrivate(EventVisibility.Public)
            .WithTimes(EventFactory.TestInterval())
            .WithMaximumNumberOfGuests(25)
            .Build();

        var result = evt.Activate(new StubCurrentTimeProvider());
        
        Assert.True(result.IsSuccess());
        Assert.Equal(EventStatus.Active, evt.Status);
    }
    
    [Fact]
    public void should_activate_ready_event()
    {
        var evt = new EventFactory().WithId(new EventId())
            .WithStatus(EventStatus.Ready)
            .WithDescription(EventFactory.TestDescription())
            .WithTitle(EventFactory.TestTitle())
            .WithIsPrivate(EventVisibility.Public)
            .WithTimes(EventFactory.TestInterval())
            .WithMaximumNumberOfGuests(25)
            .Build();

        var result = evt.Activate(new StubCurrentTimeProvider());
        
        Assert.True(result.IsSuccess());
        Assert.Equal(EventStatus.Active, evt.Status);
    }
    
    [Fact]
    public void should_not_change_status_when_activating_active_event()
    {
        var evt = new EventFactory().WithId(new EventId())
            .WithStatus(EventStatus.Active)
            .WithDescription(EventFactory.TestDescription())
            .WithTitle(EventFactory.TestTitle())
            .WithIsPrivate(EventVisibility.Public)
            .WithTimes(EventFactory.TestInterval())
            .WithMaximumNumberOfGuests(25)
            .Build();

        var result = evt.Activate(new StubCurrentTimeProvider());
        
        Assert.True(result.IsSuccess());
        Assert.Equal(EventStatus.Active, evt.Status);
    }
    
    [Fact]
    public void should_throw_if_data_is_missing()
    {
        var evt = new EventFactory().WithId(new EventId())
            .WithStatus(EventStatus.Draft)
            .WithDescription(EventFactory.TestDescription())
            .WithIsPrivate(EventVisibility.Public)
            .WithTimes(EventFactory.TestInterval())
            .WithMaximumNumberOfGuests(25)
            .Build();

        var result = evt.Activate(new StubCurrentTimeProvider());
        
        Assert.False(result.IsSuccess());
        Assert.Equal(EventStatus.Draft, evt.Status);
        Assert.Contains(Errors.TitleNotSetOrDefault(), result.Errors);
    }
    
    [Fact]
    public void should_throw_if_event_is_cancelled()
    {
        var evt = new EventFactory().WithId(new EventId())
            .WithStatus(EventStatus.Cancelled)
            .WithDescription(EventFactory.TestDescription())
            .WithIsPrivate(EventVisibility.Public)
            .WithTimes(EventFactory.TestInterval())
            .WithMaximumNumberOfGuests(25)
            .Build();

        var result = evt.Activate(new StubCurrentTimeProvider());
        
        Assert.False(result.IsSuccess());
        Assert.Equal(EventStatus.Cancelled, evt.Status);
        Assert.Contains(Errors.CancelledEventCannotBeModifiedError(), result.Errors);
    }
    
    
}