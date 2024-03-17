namespace VEA.core.tools.OperationResult;

public class Error(int status, int code, string developerMessage)
{
    public int Status { get; } = status;
    public int Code { get; } = code;
    public string DeveloperMessage { get; } = developerMessage;

    public override bool Equals(object? other)
    {
        if (other is null) return false;
        if (other.GetType() != GetType()) return false;

        return ((Error)other).DeveloperMessage.Equals(DeveloperMessage) && ((Error)other).Code.Equals(Code) &&
               ((Error)other).Status.Equals(Status);
    }
}