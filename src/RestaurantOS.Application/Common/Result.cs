namespace RestaurantOS.Application.Common;

public class Result
{
    public bool IsSuccess { get; }
    public string[] Errors { get; }

    private Result(bool isSuccess, string[] errors)
    {
        IsSuccess = isSuccess;
        Errors = errors;
    }

    public static Result Success() => new(true, []);
    public static Result Failure(params string[] errors) => new(false, errors);
}
