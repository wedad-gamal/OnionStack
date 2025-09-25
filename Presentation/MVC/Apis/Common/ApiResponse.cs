namespace MVC.Apis.Common;

public class ApiResponse<T> : IApiResponseMarker
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }

    public static ApiResponse<T> Ok(T data, string message = "Success")
        => new ApiResponse<T> { Success = true, Data = data, Message = message };

    public static ApiResponse<T> Fail(string message)
        => new ApiResponse<T> { Success = false, Message = message };
}