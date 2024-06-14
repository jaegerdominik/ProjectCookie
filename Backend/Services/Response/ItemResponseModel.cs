namespace ProjectCookie.Services.Response;

public class ItemResponseModel<T> : ResponseModel where T : class
{
    public T Data { get; set; }
}