using domain.Common.Enums;
using domain.Common.Values;

namespace UnitTests.Features.Event.UpdateDescription;

public class UpdateDescAggregateTest
{
    [Fact]
    public void should_update_description_for_a_draft()
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Draft).Build();
        
        //when
        var desc = EventDescription.Of(
            "a description"
            ).GetSuccess();
        var result = evt.UpdateDescription(desc);
        
        //then
        Assert.True(result.IsSuccess());
        Assert.Equal(desc,evt.Description);

    }
    
    [Fact]
    public void should_update_with_empty_description()
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).Build();
        
        //when
        var desc = EventDescription.Of(
            ""
        ).GetSuccess();
        var result = evt.UpdateDescription(desc);
        //then
        Assert.True(result.IsSuccess());
        Assert.Equal(desc,evt.Description);

    }
    
    [Theory]
    [InlineData("")]
    [InlineData("Nullam tempor lacus nisl, eget tempus quam maximu")]
    [InlineData("xBvpJ3nhuZyY3EVf5QDjJRU4zcfH8L2Xc3zWqfFaNTNK7KzvbQuPZS5nCKuaM6aJFHSvDNDKjd")]
    public void should_update_with_valid_arg_and_set_status_from_ready_to_draft(string description)
    {
        //given
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Ready).Build();
        
        //when
        var desc = EventDescription.Of(
            description
        ).GetSuccess();
        var result = evt.UpdateDescription(desc);
        Assert.True(result.IsSuccess());
        
        //then
        Assert.Equal(desc,evt.Description);
        Assert.Equal(EventStatus.Draft, evt.Status);

    }
    
    [Theory]
    [InlineData("2rSR5cYWTgUHnAGVb6hQQtZ2WK3VkNdeKprLXSH6i6ac8AziAPSKZEvv0Dwgc0J8Tv929CYiyvT1jgrZK02u1T6YUDS69QqJ4aR6jfJEkEt046aG7kgQ4KG0rFFCrN6pnfV3RFZwWkMvMtUBzvv4J1pUKLC7U5NR5MFe73CtLdZWKvQkxqMx4tavx6qw6bw1YeEETfiKiqTD8rY8YzDTM2cdbKDuHR1BqbMzCLx50HaDwnVNMQMM256qU4X")]
    [InlineData("CRSu4kELtTeK6dUga0yY5UrVtWZVDMV1reeHmejw2QekSQA13R92rYbymVDuuhXdF6BRtQKL6QKj1YXtaCrFQ2PBGzFddAGi71gjR9qQmyhTrBYmcCUv0bQ8CjWb9YrWxG07fB6zT9iNJH0xr45bdJaLL4ErfqNGSgtA2wmJUV1uCYxkd0bcjxftkbfbjt0nhS7tcUaFa5XGHJTqqvdaZRtHwgmPHcgQTyqrMvLwM5yJD96W9XgKpnk0ZhAwbxq2uYnB")]
    public void should_throw_when_desc_too_long(string str)
    {
        
        //when
        var result = EventDescription.Of(str);
        //then
        Assert.False(result.IsSuccess());
    }
    
    [Fact]
    public void should_throw_when_status_cancelled()
    {
        //given 
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Cancelled).Build();
        
        //when
        var desc = EventDescription.Of(
            ""
        ).GetSuccess();
        var result = evt.UpdateDescription(desc);
        //then
        Assert.False(result.IsSuccess());
        Assert.Equal("Cancelled event cannot be modified", result.Errors.First().DeveloperMessage);
    }
    
    [Fact]
    public void should_throw_when_status_active()
    {
        //given 
        var evt = new EventFactory().WithId(new EventId()).WithStatus(EventStatus.Active).Build();
        
        //when
        var desc = EventDescription.Of(
            ""
        ).GetSuccess();
        var result = evt.UpdateDescription(desc);
        //then
        Assert.False(result.IsSuccess());
        Assert.Equal("Active event cannot be modified", result.Errors.First().DeveloperMessage);
    }
}