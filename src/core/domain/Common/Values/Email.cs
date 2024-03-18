using System.Text.RegularExpressions;
using domain.Common.Bases;
using VEA.core.tools.OperationResult;

namespace domain.Common.Values;

public partial class Email : ValueObject
{
    private Email(string value)
    {
        Value = value;
    }

    protected Email()
    {
        
    }

    public string Value { get; private set; }

    public static Result<Email> Of(string value)
    {
        var validation = Validate(value);
        if (validation.IsSuccess())
            return new Result<Email>(new Email(value));

        return new Result<Email>(validation.Errors);

    }

    protected static ResultVoid Validate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
             return ResultVoid.SingleFailure(Errors.IsEmpty());
        if (!EmailRegex().IsMatch(value))
             return ResultVoid.SingleFailure(Errors.DoesNotMatchEmailPattern(value));

        return ResultVoid.Success();
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    [GeneratedRegex(@"^[^\W_](?:[\w-\.]{0,62}[^\W_])?@([^\W_][\w-]*[^\W_]\.)+[a-zA-Z]{2,6}$", RegexOptions.IgnoreCase, "en-PL")]
    private static partial Regex EmailRegex();

}