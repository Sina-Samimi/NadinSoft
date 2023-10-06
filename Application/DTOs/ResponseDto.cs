namespace WebApi.DTOs
{
    public class ResponseDto
    {
        public bool Success { get; set; }
        public string? Message { get; set; }

    }
    public class ResponseDto<T>:ResponseDto
    {
        public T? Data { get; set; }
    }
}
