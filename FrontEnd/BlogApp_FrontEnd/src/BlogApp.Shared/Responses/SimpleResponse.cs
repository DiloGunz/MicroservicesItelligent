namespace BlogApp.Shared.Responses;

public class SimpleResponse<T>
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public T Data { get; set; }
    public string Url { get; set; } = "#";

    public SimpleResponse<T> Success(T data)
    {
        IsSuccess = true;
        Data = data;
        Message = string.Empty;
        return this;
    }

    public SimpleResponse<T> Failure(string message)
    {
        IsSuccess = false;
        Message = message;
        return this;
    }

    public SimpleResponse<T> Failure(List<string> messages)
    {
        IsSuccess = false;
        string msg = string.Empty;
        foreach (var m in messages)
        {
            msg = $"- {m}\n";
        }
        Message = msg.TrimEnd('\n');
        return this;
    }
}