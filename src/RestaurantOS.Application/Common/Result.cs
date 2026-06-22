namespace RestaurantOS.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public string[] Errors { get; }

    protected Result(bool isSuccess, string[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success() => new(true, []);
    public static Result Failure(params string[] errors) => new(false, errors);
}

public class Result<T> : Result
{
    public T? Value { get; }

    private Result(bool isSuccess, T? value, string[] errors) : base(isSuccess, errors)
        => Value = value;

    public static Result<T> Success(T value) => new(true, value, []);
    public new static Result<T> Failure(params string[] errors) => new(false, default, errors);
}
