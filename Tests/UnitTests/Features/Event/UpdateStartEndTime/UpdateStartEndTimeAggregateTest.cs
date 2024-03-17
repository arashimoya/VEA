using domain.Aggregates.Events;
using domain.Common.Enums;
using domain.Common.Values;
using UnitTests.Stubs;
using VEA.core.tools.OperationResult;

namespace UnitTests.Features.Event.UpdateStartEndTime;

public class UpdateStartEndTimeAggregateTest
{
    [Theory]
    [MemberData(nameof(sameDay))]
    public void should_update_times_of_the_event_same_date(DateTime start, DateTime end)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Draft).Build();

        //when
        var result = evt.UpdateTimes(EventInterval.of(start, end).GetSuccess(), new StubCurrentTimeProvider());

        //then
        Assert.True(result.IsSuccess());
        Assert.Equal(start, evt.Interval.Start);
        Assert.Equal(end, evt.Interval.End);
    }

    [Theory]
    [MemberData(nameof(before1AM))]
    public void should_update_times_of_the_event_through_midnight(DateTime start, DateTime end)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Draft).Build();

        //when
        var result = evt.UpdateTimes(EventInterval.of(start, end).GetSuccess(), new StubCurrentTimeProvider());

        //then
        Assert.True(result.IsSuccess());
        Assert.Equal(start, evt.Interval.Start);
        Assert.Equal(end, evt.Interval.End);
    }

    [Theory]
    [MemberData(nameof(before1AM))]
    public void should_change_from_ready_to_draft_when_updating(DateTime start, DateTime end)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Draft).Build();

        //when
        var result = evt.UpdateTimes(EventInterval.of(start, end).GetSuccess(), new StubCurrentTimeProvider());

        //then
        Assert.True(result.IsSuccess());
        Assert.Equal(start, evt.Interval.Start);
        Assert.Equal(end, evt.Interval.End);
        Assert.Equal(EventStatus.Draft, evt.Status);
    }

    [Fact]
    public void should_change_when_start_in_future()
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).Build();
        var start = new DateTime(2023, 8, 25, 19, 0, 0);
        var end = new DateTime(2023, 8, 25, 23, 59, 0);

        //when
        var result = evt.UpdateTimes(EventInterval.of(start, end).GetSuccess(), new StubCurrentTimeProvider());

        //then
        Assert.True(result.IsSuccess());
        Assert.Equal(start, evt.Interval.Start);
        Assert.Equal(end, evt.Interval.End);
    }

    [Theory]
    [MemberData(nameof(before1AM))]
    public void should_change_when_duration_is_10h_or_less(DateTime start, DateTime end)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).Build();

        //when
        var result = evt.UpdateTimes(EventInterval.of(start, end).GetSuccess(), new StubCurrentTimeProvider());

        //then
        Assert.True(result.IsSuccess());
        Assert.Equal(start, evt.Interval.Start);
        Assert.Equal(end, evt.Interval.End);
    }

    [Theory]
    [MemberData(nameof(F1StartAfterEnd))]
    public void should_throw_if_start_after_end_date(DateTime start, DateTime end)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).Build();

        //when
        var result = EventInterval.of(start, end);

        //then
        Assert.False(result.IsSuccess());
        Assert.Null(evt.Interval);
        Assert.Contains(Errors.StartTimeAfterEndTime(), result.Errors);
    }

    [Theory]
    [MemberData(nameof(F2StartAfterEnd))]
    public void should_throw_if_start_date_same_as_end_date_and_start_time_after_end_time(DateTime start, DateTime end)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).Build();

        //when
        var result = EventInterval.of(start, end);

        //then
        Assert.False(result.IsSuccess());
        Assert.Null(evt.Interval);
        Assert.Contains(Errors.StartTimeAfterEndTime(), result.Errors);
    }

    [Theory]
    [MemberData(nameof(F3ShortDuration))]
    public void should_throw_if_the_same_date_but_duration_less_than_1h(DateTime start, DateTime end)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).Build();

        //when
        var result = EventInterval.of(start, end);

        //then
        Assert.False(result.IsSuccess());
        Assert.Null(evt.Interval);
        Assert.Contains(Errors.TooShortDuration(), result.Errors);
    }

    [Theory]
    [MemberData(nameof(F4ShortDuration))]
    public void should_throw_if_duration_less_than_1h(DateTime start, DateTime end)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).Build();

        //when
        var result = EventInterval.of(start, end);

        //then
        Assert.False(result.IsSuccess());
        Assert.Null(evt.Interval);
        Assert.Contains(Errors.TooShortDuration(), result.Errors);
    }

    [Theory]
    [MemberData(nameof(F5StartBefore8AM))]
    public void should_throw_if_start_before_8AM(DateTime start, DateTime end)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).Build();

        //when
        var result = EventInterval.of(start, end);

        //then
        Assert.False(result.IsSuccess());
        Assert.Null(evt.Interval);
        Assert.Contains(Errors.StartBefore8AM(), result.Errors);
    }

    [Theory]
    [MemberData(nameof(F6EndAfter1AM))]
    public void should_throw_if_end_after_1AM(DateTime start, DateTime end)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).Build();

        //when
        var result = EventInterval.of(start, end);


        //then
        Assert.False(result.IsSuccess());
        Assert.Null(evt.Interval);
        Assert.Contains(Errors.EndAfter1AM(), result.Errors);
    }

    [Fact]
    public void should_throw_for_active()
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Active).Build();
        var start = new DateTime(2023, 8, 25, 19, 0, 0);
        var end = new DateTime(2023, 8, 25, 23, 59, 0);

        //when
        var result = evt.UpdateTimes(EventInterval.of(start, end).GetSuccess(), new StubCurrentTimeProvider());

        //then
        Assert.False(result.IsSuccess());
        Assert.Null(evt.Interval);
        Assert.Contains(Errors.ActiveEventCannotBeModifiedError(), result.Errors);
    }

    [Fact]
    public void should_throw_for_cancelled()
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Cancelled).Build();
        var start = new DateTime(2023, 8, 25, 19, 0, 0);
        var end = new DateTime(2023, 8, 25, 23, 59, 0);

        //when
        var result = evt.UpdateTimes(EventInterval.of(start, end).GetSuccess(), new StubCurrentTimeProvider());

        //then
        Assert.False(result.IsSuccess());
        Assert.Null(evt.Interval);
        Assert.Contains(Errors.CancelledEventCannotBeModifiedError(), result.Errors);
    }

    [Theory]
    [MemberData(nameof(F9LongerThan10h))]
    public void should_throw_for_interval_longer_than_10h(DateTime start, DateTime end)
    {
        var evt = new EventFactory().WithId(new EventId()).Build();

        //when
        var result = EventInterval.of(start, end);

        //then
        Assert.False(result.IsSuccess());
        Assert.Null(evt.Interval);
        Assert.Contains(Errors.DurationTooLong(), result.Errors);
    }

    [Fact]
    public void should_throw_for_start_in_the_past()
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).Build();
        var interval = EventInterval.of(new DateTime(2000, 8, 25, 19, 0, 0), new DateTime(2000, 8, 25, 23, 59, 0))
            .GetSuccess();
        //when
        var result = evt.UpdateTimes(interval, new StubCurrentTimeProvider());

        //then
        Assert.False(result.IsSuccess());
        Assert.Null(evt.Interval);
        Assert.Contains(Errors.StartInThePast(), result.Errors);
    }
    
    [Theory]
    [MemberData(nameof(F11StartTimeBefore1AmAndEndTimeAfter8))]
    public void should_throw_if_spans_through_the_night(DateTime start, DateTime end)
    {
        var evt = new EventFactory().WithId(new EventId()).Build();

        //when
        var result = EventInterval.of(start, end);

        //then
        Assert.False(result.IsSuccess());
        Assert.Null(evt.Interval);
        
    }

    public static IEnumerable<object[]> sameDay()
    {
        yield return [new DateTime(2023, 8, 25, 19, 0, 0), new DateTime(2023, 8, 25, 23, 59, 0)];
        yield return [new DateTime(2023, 8, 25, 12, 0, 0), new DateTime(2023, 8, 25, 16, 30, 0)];
        yield return [new DateTime(2023, 8, 25, 8, 0, 0), new DateTime(2023, 8, 25, 12, 15, 0)];
        yield return [new DateTime(2023, 8, 25, 10, 0, 0), new DateTime(2023, 8, 25, 20, 00, 0)];
        yield return [new DateTime(2023, 8, 25, 13, 0, 0), new DateTime(2023, 8, 25, 23, 00, 0)];
    }

    public static IEnumerable<object[]> before1AM()
    {
        yield return [new DateTime(2023, 8, 25, 19, 0, 0), new DateTime(2023, 8, 26, 1, 0, 0)];
        yield return [new DateTime(2023, 8, 25, 12, 0, 0), new DateTime(2023, 8, 25, 16, 30, 0)];
        yield return [new DateTime(2023, 8, 25, 8, 0, 0), new DateTime(2023, 8, 25, 12, 15, 0)];
    }

    public static IEnumerable<object[]> F1StartAfterEnd()
    {
        yield return [new DateTime(2023, 8, 26, 19, 0, 0), new DateTime(2023, 8, 25, 1, 0, 0)];
        yield return [new DateTime(2023, 8, 26, 19, 0, 0), new DateTime(2023, 8, 25, 23, 59, 0)];
        yield return [new DateTime(2023, 8, 27, 12, 0, 0), new DateTime(2023, 8, 25, 16, 30, 0)];
        yield return [new DateTime(2023, 8, 1, 8, 0, 0), new DateTime(2023, 7, 31, 12, 15, 0)];
    }

    public static IEnumerable<object[]> F2StartAfterEnd()
    {
        yield return [new DateTime(2023, 8, 26, 19, 0, 0), new DateTime(2023, 8, 26, 14, 0, 0)];
        yield return [new DateTime(2023, 8, 26, 16, 0, 0), new DateTime(2023, 8, 26, 00, 00, 0)];
        yield return [new DateTime(2023, 8, 26, 18, 59, 0), new DateTime(2023, 8, 26, 12, 00, 0)];
        yield return [new DateTime(2023, 8, 26, 12, 0, 0), new DateTime(2023, 8, 26, 10, 10, 0)];
        yield return [new DateTime(2023, 8, 26, 8, 0, 0), new DateTime(2023, 8, 26, 0, 30, 0)];
    }

    public static IEnumerable<object[]> F3ShortDuration()
    {
        yield return [new DateTime(2023, 8, 26, 14, 0, 0), new DateTime(2023, 8, 26, 14, 50, 0)];
        yield return [new DateTime(2023, 8, 26, 18, 0, 0), new DateTime(2023, 8, 26, 18, 59, 0)];
        yield return [new DateTime(2023, 8, 26, 12, 00, 0), new DateTime(2023, 8, 26, 12, 30, 0)];
        yield return [new DateTime(2023, 8, 26, 8, 0, 0), new DateTime(2023, 8, 26, 8, 10, 0)];
    }

    public static IEnumerable<object[]> F4ShortDuration()
    {
        yield return [new DateTime(2023, 8, 25, 23, 30, 0), new DateTime(2023, 8, 26, 0, 15, 0)];
        yield return [new DateTime(2023, 8, 30, 23, 1, 0), new DateTime(2023, 8, 31, 0, 00, 0)];
        yield return [new DateTime(2023, 8, 30, 23, 59, 0), new DateTime(2023, 8, 31, 0, 1, 0)];
    }

    public static IEnumerable<object[]> F5StartBefore8AM()
    {
        yield return [new DateTime(2023, 8, 25, 7, 50, 0), new DateTime(2023, 8, 25, 14, 00, 0)];
        yield return [new DateTime(2023, 8, 25, 7, 59, 0), new DateTime(2023, 8, 25, 15, 59, 0)];
        yield return [new DateTime(2023, 8, 25, 1, 01, 0), new DateTime(2023, 8, 25, 8, 30, 0)];
        yield return [new DateTime(2023, 8, 25, 5, 59, 0), new DateTime(2023, 8, 25, 7, 59, 0)];
        yield return [new DateTime(2023, 8, 25, 0, 59, 0), new DateTime(2023, 8, 25, 7, 59, 0)];
    }

    public static IEnumerable<object[]> F6EndAfter1AM()
    {
        yield return [new DateTime(2023, 8, 24, 23, 50, 0), new DateTime(2023, 8, 25, 1, 1, 0)];
        yield return [new DateTime(2023, 8, 24, 22, 00, 0), new DateTime(2023, 8, 25, 7, 59, 0)];
        yield return [new DateTime(2023, 8, 30, 23, 01, 0), new DateTime(2023, 8, 31, 2, 30, 0)];
        yield return [new DateTime(2023, 8, 24, 23, 50, 0), new DateTime(2023, 8, 25, 1, 01, 0)];
    }
    
    public static IEnumerable<object[]> F9LongerThan10h()
    {
        yield return [new DateTime(2023, 8, 30, 8, 00, 0), new DateTime(2023, 8, 30, 18, 1, 0)];
        yield return [new DateTime(2023, 8, 30, 14, 59, 0), new DateTime(2023, 8, 31, 1, 0, 0)];
        yield return [new DateTime(2023, 8, 30, 14, 00, 0), new DateTime(2023, 8, 31, 0, 01, 0)];
        yield return [new DateTime(2023, 8, 30, 14, 00, 0), new DateTime(2023, 8, 31, 18, 30, 0)];
    }
    
    public static IEnumerable<object[]> F11StartTimeBefore1AmAndEndTimeAfter8()
    {
        yield return [new DateTime(2023, 8, 31, 00, 30, 0), new DateTime(2023, 8, 31, 8, 30, 0)];
        yield return [new DateTime(2023, 8, 30, 23, 59, 0), new DateTime(2023, 8, 31, 8, 01, 0)];
        yield return [new DateTime(2023, 8, 31, 1, 00, 0), new DateTime(2023, 8, 31, 8, 01, 0)];
    }
}