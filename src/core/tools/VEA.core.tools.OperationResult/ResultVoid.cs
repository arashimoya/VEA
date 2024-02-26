namespace VEA.core.tools.OperationResult;

public class ResultVoid
{
    private List<Error> Errors { get; }

    public ResultVoid()
    {
        Errors = new List<Error>();
    }

    public ResultVoid(List<Error> errors)
    {
        Errors = errors;
    }

    public virtual bool IsSuccess()
    {
        return Errors.Count == 0;
    }

    public static ResultVoid SingleFailure(Error error)
    {
        return new ResultVoid([error]);
    }
}