namespace ProductServer.API.Models
{
    public class RequestResult
    {
        public bool IsSuccess { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}
