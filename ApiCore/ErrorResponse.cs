namespace ApiCore
{
    public class ErrorResponse
    {
        public bool Status { get; set; } = false;
        public string Message { get; set; } = "Ocurrió un error";
        public string? Details { get; set; }
    }
}
