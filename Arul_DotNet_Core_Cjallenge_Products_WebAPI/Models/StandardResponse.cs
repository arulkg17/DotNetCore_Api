namespace DotNetCore_WebAPI.Models
{
    public class StandardResponse<T>
    {
        public bool Success { get; set; }        
        public string Message { get; set; }      
        public T Data { get; set; }              
        public List<string> Errors { get; set; } 

        public StandardResponse(bool success, string message, T data = default, List<string> errors = null)
        {
            Success = success;
            Message = message;
            Data = data;
            Errors = errors ?? new List<string>();
        }
    }
}
