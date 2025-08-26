namespace LMSConsoleApplication;

public class Result<T>
{
    public bool IsSuccess { get; }
    public T? Data { get; }
    public string ErrorMessage { get; }
    public Result(bool isFailed,string errorMessage,T? data)
    {
        IsSuccess = isFailed;
        Data = data;
        ErrorMessage = errorMessage;
    }
    
    public Result(bool isSuccess,T data)
    {
        IsSuccess = isSuccess;
        Data = data;
    }
    
    public static Result<T> Success(T data)=> new Result<T>(true,data);
    public static Result<T> Fail(string errorMessage) => new Result<T>(false,errorMessage,default);
}