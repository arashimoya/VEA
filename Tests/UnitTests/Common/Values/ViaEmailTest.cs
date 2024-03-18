using domain.Common.Values;
using VEA.core.tools.OperationResult;

namespace UnitTests.Common.Values;

public class ViaEmailTest
{
    [Theory]
    [InlineData("304777@via.dk")]
    [InlineData("adam@via.dk")]
    [InlineData("ada@via.dk")]
    public void should_create_a_via_email(string input)
    {
        var result = ViaEmail.Of(input.ToUpper());
        
        Assert.True(result.IsSuccess());
        Assert.NotNull(result.GetSuccess());
        Assert.Equal(input, result.GetSuccess().Value);
    }
    
    [Theory]
    [InlineData("dupa@gmail.com")]
    [InlineData("ps@via.dk")]
    [InlineData("12@via.dk")]
    [InlineData("123124354@via.dk")]
    [InlineData("PECORINO4@via.dk")]
    public void should_throw_if_not_a_via_email(string input)
    {
        var result = ViaEmail.Of(input);
        
        Assert.False(result.IsSuccess());
        Assert.Contains(Errors.IsNotValidViaEmail(), result.Errors);
        
    }
}