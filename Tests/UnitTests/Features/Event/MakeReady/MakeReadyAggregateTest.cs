using domain.Aggregates.Events;
using domain.Common.Enums;
using domain.Common.Values;
using UnitTests.Stubs;
using VEA.core.tools.OperationResult;

namespace UnitTests.Features.Event.MakeReady;

public class MakeReadyAggregateTest
{
    [Fact]
    public void should_make_ready()
    {
        //given 
        var evt = VeaEvent.Create();
        evt.UpdateTitle(TestTitle());
        evt.UpdateDescription(TestDescription());
        evt.UpdateTimes(TestInterval(), new StubCurrentTimeProvider());
        evt.MakePublic();
        evt.SetMaximumNumberOfGuests(10);

        //when
        var result = evt.MakeReady(new StubCurrentTimeProvider());

        //then
        Assert.True(result.IsSuccess());
        Assert.Equal(EventStatus.Ready, evt.Status);
    }

    //F1
    [Theory]
    [MemberData(nameof(F1NotSetOrDefaultValues))]
    public void should_throw_for_not_set_or_default_values(EventTitle title, EventDescription description, EventInterval interval,
        EventVisibility visibility, int maxNumberOfGuests, Error error)
    {
        //given 
        var evt1 = new EventFactory().WithId(new EventId())
            .WithStatus(EventStatus.Draft)
            .WithDescription(description)
            .WithIsPrivate(visibility)
            .WithMaximumNumberOfGuests(maxNumberOfGuests)
            .WithTimes(interval)
            .WithTitle(title)
            .Build();


        //when
        var result1 = evt1.MakeReady(new StubCurrentTimeProvider());

        Assert.False(result1.IsSuccess());
        Assert.Contains(error, result1.Errors);
        Assert.Equal(EventStatus.Draft, evt1.Status);
    }

    [Fact]
    public void should_throw_if_event_cancelled()
    {
        var evt = new EventFactory().WithId(new EventId())
            .WithStatus(EventStatus.Cancelled)
            .Build();

        var result = evt.MakeReady(new StubCurrentTimeProvider());
        
        Assert.False(result.IsSuccess());
        Assert.Contains(Errors.CancelledEventCannotBeModifiedError(), result.Errors);
        Assert.Equal(EventStatus.Cancelled, evt.Status);
    }

    [Fact]
    public void should_throw_if_start_prior_to_now()
    {
        var stub = new StubCurrentTimeProvider();
        stub.DateTime = stub.DateTime.AddHours(1);
        var evt = new EventFactory().WithId(new EventId())
            .WithStatus(EventStatus.Draft)
            .WithTimes(TestInterval())
            .Build();

        var result = evt.MakeReady(stub);
        
        Assert.False(result.IsSuccess());
        Assert.Contains(Errors.StartPriorToTimeOfReadying(), result.Errors);
        Assert.Equal(EventStatus.Cancelled, evt.Status);
    }
    
    [Fact]
    public void should_throw_if_title_is_default()
    {
        
        var evt = new EventFactory().WithId(new EventId())
            .WithStatus(EventStatus.Draft)
            .WithTitle(EventTitle.Default())
            .Build();

        var result = evt.MakeReady(new StubCurrentTimeProvider());
        
        Assert.False(result.IsSuccess());
        Assert.Contains(Errors.TitleNotSetOrDefault(), result.Errors);
        Assert.Equal(EventStatus.Draft, evt.Status);
    }


    private static EventDescription TestDescription()
    {
        return EventDescription.Of("description").GetSuccess();
    }

    private static EventInterval TestInterval()
    {
        var stub = new StubCurrentTimeProvider();
        return EventInterval.of(stub.now(), stub.now().AddHours(3)).GetSuccess();
    }

    private static EventTitle TestTitle()
    {
        return EventTitle.Of("title").GetSuccess();
    }

    public static IEnumerable<object[]> F1NotSetOrDefaultValues()
    {
        //title
        yield return
        [
            EventTitle.Default(), TestDescription(), TestInterval(), EventVisibility.Private, 25,
            Errors.TitleNotSetOrDefault()
        ];
        yield return
        [
            null!, TestDescription(), TestInterval(), EventVisibility.Private, 25, Errors.TitleNotSetOrDefault()
        ];

        //desc
        yield return
        [
            TestTitle(), EventDescription.Default(), TestInterval(), EventVisibility.Private, 25,
            Errors.DescriptionNotSetOrDefault()
        ];
        yield return
        [
            TestTitle(), null!, TestInterval(), EventVisibility.Private, 25, Errors.DescriptionNotSetOrDefault()
        ];

        //times
        yield return [TestTitle(), TestDescription(), null!, EventVisibility.Private, 25, Errors.TimesNotSet()];
        

        //maxno
        yield return
        [
            TestTitle(), TestDescription(), TestInterval(), EventVisibility.Private, 4,
            Errors.InvalidMaxNumberOfGuests()
        ];
        yield return
        [
            TestTitle(), TestDescription(), TestInterval(), EventVisibility.Private, 51,
            Errors.InvalidMaxNumberOfGuests()
        ];
    }
}