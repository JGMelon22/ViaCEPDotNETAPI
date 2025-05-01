namespace ViaCepDotNetAPI.Domains.Shared;

public class Result<T>
{
    public Result(T? data, bool isSuccess)
    {
        Data = data;
        IsSuccess = isSuccess;
    }

    public Result(T? data, bool isSuccess, string message)
    {
        Data = data;
        IsSuccess = isSuccess;
        Message = message;
    }

    public T? Data { get; }
    public bool IsSuccess { get; }
    public string Message { get; } = string.Empty;

    public static Result<T> Success(T data)
     => new Result<T>(data, true);

    public static Result<T> Failure(string message)
     => new Result<T>(default, false, message);
}