using domain.Common.Values;
using VEA.core.tools.OperationResult;

namespace UnitTests.Common.Values;

public class NameTest
{
    [Theory]
    [InlineData("Ib")]
    [InlineData("Ola")]
    [InlineData("Adam")]
    [InlineData("Piotr")]
    [InlineData("Maniek")]
    [InlineData("Zuzanna")]
    [InlineData("Gabriela")]
    [InlineData("Katarzyna")]
    [InlineData("Aleksander")]
    [InlineData("Władysława")]
    [InlineData("Arasimowicz")]
    [InlineData("Alexandraelizabethmay")]
    [InlineData("Christopheralexanderson")]
    public void should_create_name(string value)
    {
        var result = Name.Of(value);
        
        Assert.True(result.IsSuccess());
        Assert.Equal(value, result.GetSuccess().Value);
    }
    
    [Theory]
    [InlineData("I")]
    [InlineData("UvuvwevwevweOnyetenyevweUgwemuhwemOsas")]
    public void should_throw_if_not_between_2_25(string value)
    {
        var result = Name.Of(value);
        
        Assert.False(result.IsSuccess());
        Assert.Contains(Errors.NameNotBetween2And25(value.Length), result.Errors);
    }
    
    [Theory]
    [InlineData("&%&^%(*")]
    [InlineData("67550-98765")]
    public void should_throw_if_special_chars(string value)
    {
        var result = Name.Of(value);
        
        Assert.False(result.IsSuccess());
        Assert.Contains(Errors.InvalidName(), result.Errors);
    }
    
    [Theory]
    [MemberData(nameof(Capitalization))]
    public void should_capitalize(string input, string output)
    {
        var result = Name.Of(input);
        
        Assert.True(result.IsSuccess());
        Assert.Equal(output, result.GetSuccess().Value);
    }

    public static IEnumerable<object[]> Capitalization()
    {
        yield return
        [
            "adam", "Adam"
        ];
        yield return
        [
            "Adam", "Adam"
        ];
        
        yield return
        [
            "ADAM", "Adam"
        ];
        yield return
        [
            "aDAM", "Adam"
        ];
        
        yield return
        [
            "aDaM", "Adam"
        ];
        
        yield return
        [
            "AdAm", "Adam"
        ];
    }
}