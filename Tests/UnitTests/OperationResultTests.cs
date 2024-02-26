using VEA.core.tools.OperationResult;

namespace UnitTests;

public class OperationResultTests
{
    [Fact]
    public void should_instantiate_success_Result_with_void_type()
    {
        var resultVoid = new ResultVoid();

        Assert.True(resultVoid.IsSuccess());
    }

    [Fact]
    public void should_instantiate_failure_Result_with_void_type()
    {
        var error = new Error(404, 1000, "not found");
        var resultVoid = new ResultVoid([error]);

        Assert.False(resultVoid.IsSuccess());
    }

    [Fact]
    public void should_instantiate_success_Result_with_void_type_case_null()
    {
        var resultVoid = new ResultVoid([]);

        Assert.True(resultVoid.IsSuccess());
    }

    [Fact]
    public void should_instantiate_success_Result()
    {
        var result = new Result<DummyClass>(new DummyClass("test"));

        Assert.True(result.IsSuccess());
        Assert.True(result.GetSuccess() != null);
    }

    [Fact]
    public void should_instantiate_failure_Result()
    {
        var error = new Error(404, 1000, "not found");
        var result = new Result<DummyClass>([error]);

        Assert.False(result.IsSuccess());
        Assert.Throws<Exception>(() => result.GetSuccess());
    }

    [Fact]
    public void should_instantiate_failure_Result_void()
    {
        var result = ResultVoid.SingleFailure(new Error(1, 1, "Error"));

        Assert.False(result.IsSuccess());
    }

    [Fact]
    public void should_instantiate_success_Result_static()
    {
        var result = Result<DummyClass>.Success(new DummyClass("test"));

        Assert.True(result.IsSuccess());
    }

    [Fact]
    public void should_return_success()
    {
        var dummy = new DummyClass("test");
        var resultDummy = Result<DummyClass>.Success(dummy);
        var result = resultDummy.getOrElse(new DummyClass("should not return this one"));

        Assert.Equal(dummy, result);
    }

    [Fact]
    public void should_return_else()
    {
        var toReturn = new DummyClass("should return this one");
        var resultDummy = new Result<DummyClass>([new Error(404, 1000, "test")]);
        var result = resultDummy.getOrElse(toReturn);

        Assert.Equal(toReturn, result);
    }
}

class DummyClass(string dummy)
{
    private string Dummy { get; } = dummy;
}