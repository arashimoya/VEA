using domain.Aggregates.Users;
using domain.Common.Values;
using VEA.core.tools.OperationResult;

namespace UnitTests.Features.UserTest.Register;

public class CreateUserAggregateTest
{
    [Fact]
    public void should_create_user()
    {
        var viaEmail = ViaEmail.Of("USER@via.dk").GetSuccess();
        var firstName = Name.Of("user").GetSuccess();
        var lastName = Name.Of("userowski").GetSuccess();


        var result = User.Create(firstName, lastName, viaEmail);

        Assert.True(result.IsSuccess());
        Assert.NotNull(result.GetSuccess().Id);
        Assert.NotNull(result.GetSuccess().FirstName);
        Assert.NotNull(result.GetSuccess().LastName);
        Assert.NotNull(result.GetSuccess().Email);
    }
    
    [Fact]
    public void should_throw_if_not_from_via()
    {
        var viaEmail = ViaEmail.Of("USER@gmail.dk");

        Assert.False(viaEmail.IsSuccess());
        Assert.Contains(Errors.IsNotValidViaEmail(), viaEmail.Errors);
    }

    [Fact]
    public void should_throw_if_invalid_mail()
    {
        var viaEmail = ViaEmail.Of("65e46f7y8g9u9");
        
        Assert.False(viaEmail.IsSuccess());
        Assert.Contains(Errors.DoesNotMatchEmailPattern("65e46f7y8g9u9"), viaEmail.Errors);
    }

    [Fact]
    //f3 and f4
    public void should_throw_if_name_invalid_too_short()
    {
        var name = Name.Of("I");
        
        Assert.False(name.IsSuccess());
        Assert.Contains(Errors.NameNotBetween2And25("I".Length), name.Errors);
    }
    
    [Fact]
    public void should_throw_if_name_invalid_numbers()
    {
        var name = Name.Of("Adam24125");
        
        Assert.False(name.IsSuccess());
        Assert.Contains(Errors.InvalidName(), name.Errors);
    }
    
    [Fact]
    public void should_throw_if_name_invalid_symbols()
    {
        var name = Name.Of("Adam!");
        
        Assert.False(name.IsSuccess());
        Assert.Contains(Errors.InvalidName(), name.Errors);
    }
}