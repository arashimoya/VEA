namespace VEA.core.tools.OperationResult;

public class Result<T> : ResultVoid
{
    private T? Payload { get; }

    public Result(List<Error> errors) : base(errors)
    {
    }

    public Result(T? payload)
    {
        Payload = payload;
    }

    public T GetSuccess()
    {
        if (Payload != null) return Payload;

        throw new Exception("No payload inside.");
    }

    public override bool IsSuccess()
    {
        return base.IsSuccess() && Payload != null;
    }

    public static Result<T> Success(T payload)
    {
        return new Result<T>(payload);
    }

    public T GetOrElse(T toReturn)
    {
        return Payload ?? toReturn;
    }
    
    public new static Result<T> SingleFailure(Error error)
    {
        return new Result<T>([error]);
    }
}