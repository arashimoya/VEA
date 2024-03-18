using domain.Aggregates.Events;
using domain.Common.Bases;
using domain.Common.Contracts;
using domain.Common.Enums;
using domain.Common.Values;
using VEA.core.tools.OperationResult;

namespace domain.Aggregates.Users;

public class User : Aggregate<UserId>
{

    public Name FirstName { get; private set; }
    public Name LastName { get; private set; }
    public ViaEmail Email { get; private set; }

    private User(UserId id) : base(id)
    {
    }

    private User(UserId id, Name firstName, Name lastName, ViaEmail email) : base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = email;
    }


    public static Result<User> Create(Name firstName, Name lastName, ViaEmail email)
    {
        return new Result<User>(new User(new UserId(), firstName, lastName, email));
    }

    public ResultVoid Participate(VeaEvent evt, ICurrentTimeProvider currentTimeProvider)
    {
        return evt.RegisterGuestTo(this, currentTimeProvider);
    }
    

    internal User(Name firstName, Name lastName, ViaEmail viaEmail, UserId id): base(id)
    {
        FirstName = firstName;
        LastName = lastName;
        Email = viaEmail;
    }
}