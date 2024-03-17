
namespace GloBus.Infrastructure
{
    public class ApiResponse<T>
    {
        public bool Status { get; set; }
        public string Message { get; set; }
        public T Data{ get; set; }
        public string? Token { get; set; }
    }
}
