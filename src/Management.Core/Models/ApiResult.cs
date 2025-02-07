


namespace Management.Core.Models;

public class ApiResult<T>
{
    public bool Success { get; set; }
    public string Message { get; set; } = string.Empty;
    public T? Data { get; set; }
    public int StatusCode { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class GetAllResult<T> : ApiResult<T>
{
}

public class GetOneResult<T> : ApiResult<T>
{
}

public class PostResult<T> : ApiResult<T>
{
}

public class PutResult<T> : ApiResult<T>
{
}

public class DeleteResult<T> : ApiResult<T>
{
}

