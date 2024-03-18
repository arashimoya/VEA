using domain.Aggregates.Events;
using domain.Aggregates.Users;
using domain.Common.Enums;
using domain.Common.Values;
using UnitTests.Stubs;

namespace UnitTests.Features.UserTest;

public class UserFactory
{
    private Name _firstName;
    private Name _lastName;
    private ViaEmail _viaEmail;
    private UserId _id;

    public UserFactory WithId(UserId id)
    {
        _id = id;
        return this;
    }
    public UserFactory WithFirstName(Name firstName)
    {
        _firstName = firstName;
        return this;
    }
    public UserFactory WithLastName(Name lastName)
    {
        _lastName = lastName;
        return this;
    }
    
    public UserFactory WithEmail(ViaEmail email)
    {
        _viaEmail = email;
        return this;
    }
   

    public User Build()
    {
        return new User(_firstName, _lastName, _viaEmail, _id );
    }
    
    public static Name TestLastName()
    {
        return Name.Of("Adamowski").GetSuccess();
    }

    public static Name TestFirstName()
    {
        return Name.Of("Adam").GetSuccess();
    }

    public static ViaEmail TestViaEmail()
    {
        return ViaEmail.Of("adam@via.dk").GetSuccess();
    }
}