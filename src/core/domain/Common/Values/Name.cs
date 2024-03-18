using domain.Common.Bases;
using VEA.core.tools.OperationResult;

namespace domain.Common.Values;

public class Name : ValueObject
{
    
    public string Value { get; private set; }

    private Name(string value)
    {
        Value = value;
    }

    public static Result<Name> Of(string value)
    {
        var result = Validate(value);
        return result.IsSuccess() ? Result<Name>.Success(new Name(Capitalize(value))) : new Result<Name>(result.Errors);
    }

    private static ResultVoid Validate(string value)
    {
        var result = new ResultVoid();
        if (value.Length is < 2 or > 25)
        {
            result.Errors.Add(Errors.NameNotBetween2And25(value.Length));
        }

        if (!ContainsOnlyLetters(value))
        {
            result.Errors.Add(Errors.InvalidName());
        }

        return result;
    }
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private static bool ContainsOnlyLetters(string value)
    {
        return value.All(char.IsLetter);
    }

    private static string Capitalize(string value)
    {
        return char.ToUpper(value[0]) + (value.Length > 1 ? value.Substring(1).ToLower() : "");
    }
}