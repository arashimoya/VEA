using domain.Common.Values;
using VEA.core.tools.OperationResult;

namespace UnitTests.Common.Values;

public class EmailTest
{
    [Fact]
    public void should_create_an_email_from_string()
    {
        //given
        var str = "example@exampledomain.com";
        
        //when
        var result = Email.Of(str);
        
        //then
        Assert.True(result.IsSuccess());
        Assert.NotNull(result.GetSuccess());
        Assert.NotNull(result.GetSuccess().Value);
        Assert.Equal(str, result.GetSuccess().Value);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("    ")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData("\r")]
    [InlineData("\r\n")]
    [InlineData(" \t\r\n")]
    public void should_throw_if_empty_str(string str)
    {

        //when
        var result = Email.Of(str);
        
        //then
        Assert.False(result.IsSuccess());
        Assert.Contains(Errors.IsEmpty(), result.Errors);
    }
    
    [Theory]
    [InlineData("example.com")]
    [InlineData("example@com")]
    [InlineData("@example.com")]
    [InlineData("example@example..com")]
    [InlineData("example@.com")]
    [InlineData("example@com.")]
    [InlineData(" example@example.com")]
    [InlineData("example@example.com ")]
    [InlineData("example@ example.com")]
    [InlineData("example@@example.com")]
    [InlineData("example")]
    [InlineData(".example@example.com")]
    [InlineData("example@-example.com")]
    [InlineData("example@example.com-")]
    [InlineData("example@example")]
    public void should_throw_if_invalid_email(string str)
    {

        //when
        var result = Email.Of(str);
        
        //then
        Assert.False(result.IsSuccess());
        Assert.Contains(Errors.DoesNotMatchEmailPattern(str), result.Errors);
    }
}