namespace VEA.core.tools.OperationResult;

public class Error(int status, int code, string developerMessage)
{
    public int Status { get; } = status;
    public int Code { get; } = code;
    public string DeveloperMessage { get; } = developerMessage;

    
}