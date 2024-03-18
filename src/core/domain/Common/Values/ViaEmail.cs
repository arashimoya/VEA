using System.Text.RegularExpressions;
using VEA.core.tools.OperationResult;

namespace domain.Common.Values;

public partial class ViaEmail : Email
{
    public new string Value { get; private set; }

    private ViaEmail(string value)
    {
        Value = value;
    }

    public static Result<ViaEmail> Of(string value)
    {
        var emailResult = Email.Validate(value);

        if (!emailResult.IsSuccess()) return new Result<ViaEmail>(emailResult.Errors);
        var viaResult = Validate(value);
        return viaResult.IsSuccess() ? new Result<ViaEmail>(new ViaEmail(value.ToLower())) : new Result<ViaEmail>(viaResult.Errors);

    }

    private new static ResultVoid Validate(string value)
    {
        var text1 = value.Split("@")[0];
        var text2 = value.Split("@")[1];

        if ((ContainsOnlyThreeOrFourEnglishLetters(text1) || ContainsOnlySixDigits(text1)) && IsViaDomain(text2))
        {
            return ResultVoid.Success();
        }

        return ResultVoid.SingleFailure(Errors.IsNotValidViaEmail(value));
    }

    private static bool ContainsOnlyThreeOrFourEnglishLetters(string input)
    {
        return ThreeOrFourEnglishLetter().IsMatch(input);
    }

    private static bool ContainsOnlySixDigits(string input)
    {
        return SixDigits().IsMatch(input);
    }

    private static bool IsViaDomain(string input)
    {
        return input.ToLower().Equals("via.dk");
    }

    [GeneratedRegex(@"^[A-Za-z]{3,4}$")]
    private static partial Regex ThreeOrFourEnglishLetter();

    [GeneratedRegex(@"^\d{6}$")]
    private static partial Regex SixDigits();
}