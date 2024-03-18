using domain.Aggregates.Users;
using domain.Common.Enums;
using domain.Common.Values;
using UnitTests.Features.Event;
using UnitTests.Stubs;
using VEA.core.tools.OperationResult;

namespace UnitTests.Features.UserTest.ParticipateInPublicEvent;

public class ParticipateInPublicEventAggregateTest
{
    [Fact]
    public void should_participate_in_event()
    {
        var evt = new EventFactory()
            .WithStatus(EventStatus.Active)
            .WithIsPrivate(EventVisibility.Public)
            .WithMaximumNumberOfGuests(20)
            .WithTimes(EventFactory.TestInterval())
            .Build();

        var user = new UserFactory()
            .WithId(new UserId())
            .Build();

        var result = user.Participate(evt, new StubCurrentTimeProvider());
        
        Assert.True(result.IsSuccess());
        Assert.Contains(user, evt.Guests);
    }
    
    [Theory]
    [InlineData(EventStatus.Draft)]
    [InlineData(EventStatus.Ready)]
    [InlineData(EventStatus.Cancelled)]
    public void should_throw_when_trying_to_participate_in_non_active_event(EventStatus status)
    {
        var evt = new EventFactory()
            .WithStatus(status)
            .WithIsPrivate(EventVisibility.Public)
            .Build();

        var user = new UserFactory()
            .WithId(new UserId())
            .Build();

        var result = user.Participate(evt, new StubCurrentTimeProvider());
        
        Assert.False(result.IsSuccess());
        Assert.Contains(Errors.EventNotActive(), result.Errors);
        Assert.DoesNotContain(user, evt.Guests);
    }
    
    [Fact]
    public void should_throw_when_trying_to_participate_in_full_event()
    {
        var evt = new EventFactory()
            .WithStatus(EventStatus.Active)
            .WithIsPrivate(EventVisibility.Public)
            .WithMaximumNumberOfGuests(1)
            .WithGuests([EventFactory.TestUser()])
            .Build();

        var user = new UserFactory()
            .WithId(new UserId())
            .Build();

        var result = user.Participate(evt, new StubCurrentTimeProvider());
        
        Assert.False(result.IsSuccess());
        Assert.Contains(Errors.CapacityReached(), result.Errors);
        Assert.DoesNotContain(user, evt.Guests);
    }
    
    [Fact]
    public void should_throw_when_trying_to_participate_in_ongoing_event()
    {
        var evt = new EventFactory()
            .WithStatus(EventStatus.Active)
            .WithIsPrivate(EventVisibility.Public)
            .WithTimes(EventFactory.TestInterval())
            .WithGuests([EventFactory.TestUser()])
            .Build();

        var user = new UserFactory()
            .WithId(new UserId())
            .Build();

        var stub = new StubCurrentTimeProvider();
        stub.DateTime = stub.now().AddYears(2);

        var result = user.Participate(evt, stub);
        
        Assert.False(result.IsSuccess());
        Assert.Contains(Errors.OngoingEvent(), result.Errors);
        Assert.DoesNotContain(user, evt.Guests);
    }
    
    [Fact]
    public void should_throw_when_trying_to_participate_in_private_event()
    {
        var evt = new EventFactory()
            .WithStatus(EventStatus.Active)
            .WithIsPrivate(EventVisibility.Private)
            .WithTimes(EventFactory.TestInterval())
            .WithGuests([EventFactory.TestUser()])
            .Build();

        var user = new UserFactory()
            .WithId(new UserId())
            .Build();

        var stub = new StubCurrentTimeProvider();

        var result = user.Participate(evt, stub);
        
        Assert.False(result.IsSuccess());
        Assert.Contains(Errors.PrivateEvent(), result.Errors);
        Assert.DoesNotContain(user, evt.Guests);
    }
    
    [Fact]
    public void should_throw_when_trying_to_participate_in_already_participated_event()
    {
        var user = new UserFactory()
            .WithId(new UserId())
            .Build();
        
        var evt = new EventFactory()
            .WithStatus(EventStatus.Active)
            .WithIsPrivate(EventVisibility.Public)
            .WithTimes(EventFactory.TestInterval())
            .WithGuests([user])
            .Build();


        var stub = new StubCurrentTimeProvider();

        var result = user.Participate(evt, stub);
        
        Assert.False(result.IsSuccess());
        Assert.Contains(Errors.AlreadyParticipating(), result.Errors);
        Assert.Contains(user, evt.Guests);
    }
}