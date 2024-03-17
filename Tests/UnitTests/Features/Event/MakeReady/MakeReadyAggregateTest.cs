using domain.Common.Enums;
using domain.Common.Values;
using VEA.core.tools.OperationResult;

namespace UnitTests.Features.Event.MakeReady;

public class MakeReadyAggregateTest
{
    [Fact]
    public void should_make_ready()
    {
        //given 
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Draft).Build();
        
        //when
        var result = evt.MakeReady();
        
        //then
        Assert.True(result.IsSuccess());
        Assert.Equal(EventStatus.Ready, evt.Status);
    }

    //F1
    [Fact]
    public void should_fail_for_not_set_title()
    {
        //given 
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Draft).WithTitle(null!).Build();
        
        //when
        var result = evt.MakeReady();
        
        Assert.False(result.IsSuccess());
        Assert.Equal(Errors.TitleNotSet(), result.Errors.First());
    }
    
    [Fact]
    public void should_fail_for_default_title()
    {
        //given 
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Draft).WithTitle(EventTitle.DefaultTitle()).Build();
        
        //when
        var result = evt.MakeReady();
        
        Assert.False(result.IsSuccess());
        Assert.Equal(Errors.DefaultTitle(), result.Errors.First());
    }
    
    [Fact]
    public void should_fail_for_not_set_description()
    {
        //given 
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Draft).WithDescription(null!).Build();
        
        //when
        var result = evt.MakeReady();
        
        Assert.False(result.IsSuccess());
        Assert.Equal(Errors.DescriptionNotSet(), result.Errors.First());
    }
    
    [Fact]
    public void should_fail_for_default_description()
    {
        //given 
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Draft).WithDescription(EventDescription.DefaultDescription()).Build();
        
        //when
        var result = evt.MakeReady();
        
        Assert.False(result.IsSuccess());
        Assert.Equal(Errors.DefaultDescription(), result.Errors.First());
    }
    
    
}